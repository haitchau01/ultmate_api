using Constracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using Presentation.ActionFilters;
using Service;
using Shared.DataTransferObjects;
using ultimate_api.Extensions;
using ultimate_api.Utility;

var builder = WebApplication.CreateBuilder(args);

// nlog config
var nLogConfigPath = string.Concat(Directory.GetCurrentDirectory(), "/nlog.config");
if (File.Exists(nLogConfigPath))
{ 
    LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); 
}

//start -- Add services to the container.

//start -- ultimate_api.Extensions

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureVersioning();

builder.Services.AddScoped<IDataShaper<UserDTO>, DataShaper<UserDTO>>();
builder.Services.AddScoped<IUserLinks, UserLinks>();

//end -- ultimate_api.Extensions
//action filter
builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//Config reponse
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    // Add the NewtonsoftJsonPatchInputFormatter for JSON Patch
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
})
.AddXmlDataContractSerializerFormatters()
.AddCustomCSVFormatter()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

builder.Services.AddCustomMediaTypes();

//end -- Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

// Local function to get the NewtonsoftJsonPatchInputFormatter
NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection()
        .AddLogging()
        .AddMvc()
        .AddNewtonsoftJson()
        .Services
        .BuildServiceProvider()
        .GetRequiredService<IOptions<MvcOptions>>()
        .Value
        .InputFormatters
        .OfType<NewtonsoftJsonPatchInputFormatter>()
        .First();
