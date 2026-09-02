using Core.Contract.DTOs.Operator;
using Core.Contract.DTOs.User;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;

namespace Host.Controllers;


[ApiController]
[Route("api/[controller]")]
public class OperatorController(IOperator oper) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPagination([FromQuery] PaginationParams param)
      {
            var res = await oper.GetPaginationAsync(param);
            return Ok(res);

      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody] CreateOperatorDto dto)
      {
            var res = await oper.CreateAsync(dto);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody] UpdateOperatorDto dto)
      {
            var res = await oper.UpdateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{guid}")]
      public async Task<IActionResult> DeleteByGuidAsync(Guid guid)
      {
            var res = await oper.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpGet("image/{guid}")]
      [Produces("image/png")]
      public async Task<IActionResult> GetImageAsync(Guid guid)
      {
            var stream = await oper.GetImageByGuidAsync(guid);
            return File(stream, "image/png");
      }

      [HttpPost("image/upload/{guid}")]
      [Consumes("multipart/form-data")]
      public async Task<IActionResult> UploadImageAsync([FromForm] UploadImageDto request, Guid guid)
      {
            await oper.UploadImageAsync(guid, request.Image.OpenReadStream());
            return Ok();
      }



}