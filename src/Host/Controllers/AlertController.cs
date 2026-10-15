using Core.Contract.DTOs.Alert;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

public class AlertController(IAlert alert) : ControllerBase
{
      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync(PaginationParams param)
      {
            var res = await alert.GetPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("event/pagination")]
      public async Task<IActionResult> GetEventPaginationAsync(PaginationParams param)
      {
            var res = await alert.GetEventPaginationAsync(param);
            return Ok(res);
      }

      [HttpGet("{guid}")]
      public async Task<IActionResult> GetAsync(Guid guid)
      {
            var res = await alert.GetByGuidAsync(guid);
            return Ok(res);
      }

      [HttpPost]
      public async Task<IActionResult> CreateAsync([FromBody] CreateAlertDto dto)
      {
            var res = await alert.CreateAsync(dto);
            return Ok(res);
      }

      [HttpPut]
      public async Task<IActionResult> UpdateAsync([FromBody] UpdateAlertDto dto)
      {
            var res = await alert.UpdateAsync(dto);
            return Ok(res);
      }

      [HttpDelete("{guid}")]
      public async Task<IActionResult> DeleteAsync(Guid guid)
      {
            var res = await alert.DeleteByGuidAsync(guid);
            return Ok(res);
      }

      [HttpDelete("range")]
      public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Guid> guids)
      {
            var res = await alert.DeleteListAsync(guids);
            return Ok(res);
      }

      [HttpPatch("enable/{guid}")]
      public async Task<IActionResult> EnableAsync(Guid guid)
      {


            var res = await alert.EnabledAsync(guid);
            return Ok(res);
      }

      [HttpPatch("disable/{guid}")]
      public async Task<IActionResult> DisableAsync(Guid guid)
      {


            var res = await alert.DisabledAsync(guid);
            return Ok(res);
      }
}