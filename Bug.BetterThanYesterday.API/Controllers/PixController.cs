using Bug.BetterThanYesterday.Application.Payments.ConfirmPixPayment;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[Route("testapi/[controller]")]
[ApiController]
public class PixController(IUseCase<ConfirmPixPaymentCommand> confirmPixPaymentUseCase) : ControllerBase
{
	[HttpPost("Webhook")]
	public async Task<IActionResult> Webhook([FromBody] PixWebhookRequest request)
	{
		var result = await confirmPixPaymentUseCase.HandleAsync(new ConfirmPixPaymentCommand
		{
			ExternalPaymentId = request.ExternalPaymentId
		});

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}

public sealed class PixWebhookRequest
{
	public string ExternalPaymentId { get; set; } = string.Empty;
}
