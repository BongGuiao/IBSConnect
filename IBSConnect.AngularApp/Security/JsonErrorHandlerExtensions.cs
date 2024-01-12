using System;
using System.Security.Authentication;
using IBSConnect.Business.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace IBSConnect.AngularApp.Security;

public static class JsonErrorHandlerExtensions
{
    public static void UserJsonErrorHandler(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                //TODO: Handle nested exceptions

                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();

                string json;


                switch (exceptionHandlerPathFeature.Error)
                {
                    case InvalidCredentialException invalidCredentialException:
                    {
                        context.Response.StatusCode = 401;
                        var err = new
                        {
                            type = "authentication",
                            message = "Invalid username or password",
                        };
                        json = JsonConvert.SerializeObject(err);
                        break;

                    }
                    case UnauthorizedAccessException unauthorizedAccessException:
                    {
                        context.Response.StatusCode = 403;
                        var err = new
                        {
                            type = "authorization",
                            message = "You don't have access to this resource",
                        };
                        json = JsonConvert.SerializeObject(err);
                        break;
                    }
                    // Handle validation errors
                    case ValidationException validationException:
                    {
                        context.Response.StatusCode = 400;
                        var err = new
                        {
                            type = "validation",
                            messages = validationException.Messages
                        };
                        json = JsonConvert.SerializeObject(err);
                        break;
                    }
                    // Handle known errors
                    case IBSConnectException IBSConnectException:
                    {
                        context.Response.StatusCode = 400;
                        var err = new
                        {
                            type = "error",
                            message = IBSConnectException.Message,
                        };
                        json = JsonConvert.SerializeObject(err);
                        break;
                    }
                    default:
                    {
                        // Handle unknown errors
                        if (env.IsDevelopment())
                        {
                            var err = new
                            {
                                type = "exception",
                                message = exceptionHandlerPathFeature.Error.Message,
                                stacktrace = exceptionHandlerPathFeature.Error.StackTrace
                            };
                            json = JsonConvert.SerializeObject(err);
                        }
                        else
                        {
                            // Don't display the message or stacktrace in Production
                            var err = new
                            {
                                type = "exception",
                                message = "An unexpected error occured. Please contact your Administrator",
                            };
                            json = JsonConvert.SerializeObject(err);

                            logger.Error(exceptionHandlerPathFeature.Error, "An error occured");
                        }

                        break;
                    }
                }

                await context.Response.WriteAsync(json);
            });
        });

    }
}