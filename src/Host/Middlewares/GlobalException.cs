using System;
using System.Net;
using SharedKernel.Domain;
using SharedKernel.Exceptions;

namespace Host.Middlewares;

public sealed class GlobalException : IMiddleware
{
      public async Task InvokeAsync(HttpContext context, RequestDelegate next)
      {
            try
            {
                  await next(context);
            }
            catch (Exception ex)
            {
                  await ExceptionSwitcher(context, ex);
            }
      }

      private async Task ExceptionSwitcher(HttpContext context, Exception ex)
      {
            switch (ex)
            {
                  case BadRequestException:
                  case ArgumentException:
                        await BadRequestExceptionHandler(context, ex);
                        break;
                  case UnauthorizedAccessException:
                        await UnauthorizedAccessExceptionHandler(context, ex);
                        break;
                  case ForbiddenException:
                        await ForbiddenExceptionHandler(context, ex);
                        break;
                  case NotFoundException:
                        await NotFoundExceptionHandler(context, ex);
                        break;
                  default:
                        await HandleException(context, ex);
                        break;
            }
      }

      private Task BadRequestExceptionHandler(HttpContext context, Exception ex)
      {
            // Log the exception (you can use a logging framework here)
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Set the response status code and content
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new BaseResponse<object>(
                  DateTime.UtcNow,
                  System.Net.HttpStatusCode.BadRequest,
                  false, 
                  "Bad request",
                  Errors:new SharedKernel.Model.BaseErrorResponse(
                        ex.Message
                  ));

            return context.Response.WriteAsJsonAsync(response);


      }


      private Task UnauthorizedAccessExceptionHandler(HttpContext context, Exception ex)
      {
            // Log the exception (you can use a logging framework here)
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Set the response status code and content
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new BaseResponse<object>(
                  DateTime.UtcNow,
                  System.Net.HttpStatusCode.Unauthorized,
                  false, 
                  "Unauthorized",
                  Errors:new SharedKernel.Model.BaseErrorResponse(
                        ex.Message
                  ));

            return context.Response.WriteAsJsonAsync(response);
      }

      private Task NotFoundExceptionHandler(HttpContext context, Exception ex)
      {
            // Log the exception (you can use a logging framework here)
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Set the response status code and content
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            var response = new BaseResponse<object>(
                  DateTime.UtcNow,
                  System.Net.HttpStatusCode.NotFound,
                  false, 
                  "Not found",
                  Errors:new SharedKernel.Model.BaseErrorResponse(
                        ex.Message
                  ));
            return context.Response.WriteAsJsonAsync(response);
      }

      private Task ForbiddenExceptionHandler(HttpContext context, Exception ex)
      {
            // Log the exception (you can use a logging framework here)
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Set the response status code and content
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var response = new BaseResponse<object>(
                  DateTime.UtcNow,
                  System.Net.HttpStatusCode.Forbidden,
                  false, 
                  "Forbidden",
                  Errors:new SharedKernel.Model.BaseErrorResponse(
                        ex.Message
                  ));

            return context.Response.WriteAsJsonAsync(response);
      }

      private Task HandleException(HttpContext context, Exception ex)
      {
            // Log the exception (you can use a logging framework here)
            Console.WriteLine($"An error occurred: {ex.Message}");

            // Set the response status code and content
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            if (ex.InnerException is null)
            {
                  return context.Response.WriteAsJsonAsync(
                         new BaseResponse<object>(
                              DateTime.UtcNow,
                              System.Net.HttpStatusCode.InternalServerError,
                              false, 
                              "Internal server error",
                              Errors:new SharedKernel.Model.BaseErrorResponse(
                                    ex.Message
                              ))
                  );
            }
            else
            {
                   return context.Response.WriteAsJsonAsync(
                         new BaseResponse<object>(
                              DateTime.UtcNow,
                              System.Net.HttpStatusCode.InternalServerError,
                              false, 
                              "Internal server error",
                              Errors:new SharedKernel.Model.BaseErrorResponse(
                                    ex.Message,
                                    ex.InnerException.ToString(),
                                    ex.StackTrace
                              ))
                  );
            }

      }
}
