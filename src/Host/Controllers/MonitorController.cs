using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[Route("api/notifications")]
[ApiController]
public class MonitorController : ControllerBase
{
      [HttpPost]
      public IActionResult Dao([FromBody]object body)
      {
            Console.WriteLine(body);
            return Ok();
      }

      [HttpPost("dao")]
      public IActionResult Dao2([FromBody]object body)
      {
            Console.WriteLine(body);
            return Ok();
      }
}