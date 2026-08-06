using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;

namespace Bug.BetterThanYesterday.API;

public sealed class RequestLoggingMiddleware : IMiddleware
{
	private const int MaxBodyLength = 2048;
	private static readonly string[] SkippedPathPrefixes = ["/swagger", "/favicon.ico"];

	private readonly ILogger<RequestLoggingMiddleware> _logger;
	private readonly IHostEnvironment _environment;

	public RequestLoggingMiddleware(
		ILogger<RequestLoggingMiddleware> logger,
		IHostEnvironment environment)
	{
		_logger = logger;
		_environment = environment;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		if (!_environment.IsDevelopment() || ShouldSkip(context.Request.Path))
		{
			await next(context);
			return;
		}

		var requestBody = await ReadRequestBodyAsync(context.Request);
		var originalBodyStream = context.Response.Body;

		using var responseBodyStream = new MemoryStream();
		context.Response.Body = responseBodyStream;

		await next(context);

		var responseBody = await ReadStreamAsTextAsync(responseBodyStream);
		await responseBodyStream.CopyToAsync(originalBodyStream);
		context.Response.Body = originalBodyStream;

		var logLine = BuildLogLine(context, requestBody, responseBody);
		_logger.LogInformation("{LogLine}", logLine);
		await WriteToFileAsync(logLine);
	}

	private static bool ShouldSkip(PathString path)
	{
		var value = path.Value ?? string.Empty;
		return SkippedPathPrefixes.Any(prefix =>
			value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
	}

	private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
	{
		if (!request.ContentLength.HasValue || request.ContentLength == 0)
			return string.Empty;

		var contentType = request.ContentType ?? string.Empty;
		if (!contentType.Contains("json", StringComparison.OrdinalIgnoreCase))
			return $"(skipped non-json body: {contentType})";

		request.EnableBuffering();
		request.Body.Position = 0;

		using var reader = new StreamReader(
			request.Body,
			Encoding.UTF8,
			detectEncodingFromByteOrderMarks: false,
			leaveOpen: true);

		var body = await reader.ReadToEndAsync();
		request.Body.Position = 0;

		return Truncate(body);
	}

	private static async Task<string> ReadStreamAsTextAsync(Stream stream)
	{
		stream.Position = 0;
		using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
		var body = await reader.ReadToEndAsync();
		stream.Position = 0;
		return Truncate(body);
	}

	private static string BuildLogLine(HttpContext context, string requestBody, string responseBody)
	{
		var request = context.Request;
		var statusCode = context.Response.StatusCode;
		var query = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;

		return string.Join(
			Environment.NewLine,
			$"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {request.Method} {request.GetDisplayUrl()}",
			$"Request body: {(requestBody.Length == 0 ? "(empty)" : requestBody)}",
			$"Response {statusCode}: {(responseBody.Length == 0 ? "(empty)" : responseBody)}");
	}

	private async Task WriteToFileAsync(string logLine)
	{
		var logsDirectory = Path.Combine(_environment.ContentRootPath, "logs");
		Directory.CreateDirectory(logsDirectory);

		var logPath = Path.Combine(logsDirectory, "api.log");
		await File.AppendAllTextAsync(logPath, logLine + Environment.NewLine + Environment.NewLine);
	}

	private static string Truncate(string value)
	{
		if (string.IsNullOrEmpty(value))
			return string.Empty;

		if (value.Length <= MaxBodyLength)
			return value;

		return value[..MaxBodyLength] + "... (truncated)";
	}
}
