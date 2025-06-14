using Microsoft.AspNetCore.Mvc;

namespace MyDiary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetNTID()
    {
        return Ok(Environment.UserName);
    }
}