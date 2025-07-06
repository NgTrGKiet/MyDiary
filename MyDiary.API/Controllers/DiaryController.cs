using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDiary.Application.Diary.Commands.CreateDiary;
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
    }
}
