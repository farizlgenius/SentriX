using Core.Contract.DTOs.Location;
using Core.Contract.Interfaces.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController(ILocation loc) : ControllerBase
    {
        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
        {
            var res = await loc.GetPaginationAsync(param);
            return Ok(res);
        }


        [HttpGet("countries")]
        public async Task<IActionResult> GetAllCountryAsync()
        {
            var res = await loc.GetCountriesAsync();
            return Ok(res);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetByGuidAsync(Guid guid)
        {
            var res = await loc.GetByGuidAsync(guid);
            return Ok(res);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateLocationDto dto)
        {
            var res = await loc.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteByGuidAsync(Guid guid)
        {
            var res = await loc.DeleteByGuidAsync(guid);
            return Ok(res);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateLocationDto dto)
        {
            var res = await loc.UpdateAsync(dto);
            return Ok(res);
        }
    }
}
