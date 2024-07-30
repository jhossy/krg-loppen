using Microsoft.AspNetCore.Diagnostics;
using System.Text;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
	private readonly ILogger<GlobalExceptionHandler> _logger;

	public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		_logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

		httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

		await httpContext.Response.WriteAsync(File.ReadAllText("./Views/Error/500.cshtml"), Encoding.GetEncoding("iso-8859-1"), cancellationToken);

		return true;
	}
}
