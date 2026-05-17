using Microsoft.AspNetCore.Mvc;
using Output.Contract.DTOs;
using SharedKernel.Domain;

namespace Host.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      public class OutputController(IOutput output) : ControllerBase
      {
            [HttpGet]
            public async Task<IActionResult> GetPagination([FromQuery]PaginationParams param)
            {
                  var res = await output.GetPaginationAsync(param);
                  return Ok(res);
            }

            [HttpPost]
            public async Task<IActionResult> CreateAsync([FromBody] CreateOutputDto dto)
            {
                  var res = await output.CreateAsync(dto);
                  return Ok(res);
            }

      }

}

