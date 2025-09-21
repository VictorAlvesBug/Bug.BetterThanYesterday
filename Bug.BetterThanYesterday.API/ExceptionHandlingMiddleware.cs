using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API
{
	public sealed class ExceptionHandlingMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await WriteProblem(context, StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		private static Task WriteProblem(
			HttpContext context,
			int statusCode,
			string errorMessage)
		{
			context.Response.StatusCode = statusCode;
			context.Response.ContentType = "application/problem+json";
			
			var problemDetails = new ProblemDetails
			{
				Type = "about:blank",
				Status = statusCode,
				Title = errorMessage,
				Detail = errorMessage
			};

			return context.Response.WriteAsJsonAsync(problemDetails);
		}
	}
}
