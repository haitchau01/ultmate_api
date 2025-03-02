using Constracts;
using Entities.ErrorModels;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Exceptions;

namespace ultimate_api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Error handling with middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="logger"></param>
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
        {
            //regist exception
            app.UseExceptionHandler(appError =>
            {
                //when an exception occurs this midleware called
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    // get detail exception
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        // for details err
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => (int)HttpStatusCode.NotFound,
                            ValidationException => (int)HttpStatusCode.BadRequest,
                            Exceptions.UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                            _ => (int)HttpStatusCode.InternalServerError
                        };

                        //add to logger
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        //return json for client
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
