using ApplicationCore.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationCore.Middlewares
{
	public class ErrorDetails
	{
		public int StatusCode { get; set; }
		public string Message { get; set; }


		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}

	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IAppLogger _logger;

		public ExceptionMiddleware(RequestDelegate next, IAppLogger logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogException(ex);
				
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			return context.Response.WriteAsync(new ErrorDetails()
			{
				StatusCode = context.Response.StatusCode,
				Message = "伺服器暫時無回應. 請稍候再試"
			}.ToString());
		}

	}
}
