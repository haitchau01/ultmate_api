using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using ultimate_api.Extensions;


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

//end -- ultimate_api.Extensions


builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

//end -- Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
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
