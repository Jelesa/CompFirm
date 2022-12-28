using CompFirm.Domain.Exceptions;
using CompFirm.Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompFirm
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        private readonly Type[] KnownExceptions = new Type[]
        {
            typeof(CustomException)
        };

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                if (!httpContext.Request.Path.Value.Contains("/api"))
                {
                    throw;
                }

                httpContext.Response.StatusCode = 500;

                var message = ex.Message;

                if (ex is ApiException apiException)
                {
                    httpContext.Response.StatusCode = (int)apiException.HttpStatusCode;
                }

                if (KnownExceptions.Contains(ex.GetType()))
                {
                    httpContext.Response.StatusCode = 400;
                }

                if (ex is MySqlException)
                {
                    message = "Ошибка соединения с базой данных.";
                }

                var responseBody = JsonConvert.SerializeObject(new ExceptionResponseDto
                {
                    Message = message
                });

                httpContext.Response.Headers.Add("Content-Type", "application/json");

                await httpContext.Response.WriteAsync(responseBody);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
