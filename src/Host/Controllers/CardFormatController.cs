using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class CardFormatController(ICardFormat card) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync([FromQuery]PaginationParams param)
      {
            var res = card.GetPaginationAsync(param);
            return Ok(res);
      }
}