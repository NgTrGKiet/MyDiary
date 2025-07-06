using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDiary.Application.Diary.Commands.CreateDiary;
using MyDiary.Application.Diary.Commands.DeleteDiary;
using MyDiary.Application.Diary.Commands.UpdateDiary;
using MyDiary.Application.Diary.Queries.GetDiaryById;
using System.Security.Claims;

namespace MyDiary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiaryController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{diaryId:guid}")]
        public async Task<IActionResult> GetDiaryById([FromRoute] Guid diaryId)
        {
            var result = await mediator.Send(new GetDiaryByIdQuery(diaryId));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiary(CreateDiaryCommand request)
        {
            Guid diaryId = await mediator.Send(request);
            return CreatedAtAction(nameof(GetDiaryById), new { diaryId }, diaryId);
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDiary(UpdateDiaryCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{diaryId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDiary([FromRoute] Guid diaryId)
        {
            await mediator.Send(new DeleteDiaryCommand(diaryId));
            return NoContent();
        }
    }
}
