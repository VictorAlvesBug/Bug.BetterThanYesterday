using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Uploads;
using Bug.BetterThanYesterday.Application.Uploads.GeneratePresignedUploadUrl;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[Route("testapi/[controller]")]
[ApiController]
public class UploadsController(IUseCase<GeneratePresignedUploadUrlCommand> generatePresignedUploadUrlUseCase)
	: ControllerBase
{
	[HttpPost(nameof(PresignedUrl))]
	public async Task<IActionResult> PresignedUrl([FromBody] GeneratePresignedUploadUrlCommand command)
	{
		var result = await generatePresignedUploadUrlUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<PresignedUploadUrlModel>)result).Data;
			return Created($"Uploads/{nameof(PresignedUrl)}", result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
