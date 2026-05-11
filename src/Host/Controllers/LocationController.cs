using Location.Contract.DTOs;
using Location.Contract.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController(ILocation location) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var res = await location.GetAsync();
            return Ok(res);
        }

        [HttpPost("range")]
        public async Task<IActionResult> GetRangeLocationAsync([FromBody] RangeIdDto dto)
        {
            var res = await location.GetRangeLocationAsync(dto);
            return Ok(res);
        }

        [HttpGet("pagination/countries")]
        public async Task<IActionResult> GetCountriesAsync([FromQuery] int Page,[FromQuery] int PageSize,[FromQuery] string? Search)
        {
            var res = await location.GetCountriesPaginationAsync(Page, PageSize, Search ?? "");
            return Ok(res);
        }

        [HttpGet("country")]
        public async Task<IActionResult> GetAllCountryAsync()
        {
            var res = await location.GetAllCountriesAsync();
            return Ok(res);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaginationAsync([FromQuery] int Page, [FromQuery] int PageSize, [FromQuery] string? Search)
        {
            var res = await location.GetPaginationAsync(Page, PageSize, Search ?? "");
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateLocationDto dto)
        {
            var res = await location.CreateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            var res = await location.DeleteByIdAsync(id);
            return Ok(res);
        }

        [HttpPost("delete/range")]
        public async Task<IActionResult> DeleteRangeAsync([FromBody] RangeIdDto dto)
        {
            var res = await location.DeleteRangeAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateLocationDto dto)
        {
            var res = await location.UpdateAsync(dto);
            return Ok(res);
        }
    }
}
