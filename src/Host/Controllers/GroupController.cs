using System.Security.Claims;
using Core.Contract.DTOs.Group;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController(IGroup group) : ControllerBase
{
    [HttpGet("pagination")]
    public async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationParams param)
    {
        var res = await group.GetPaginationAsync(param);
        return Ok(res);
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetByGuidAsync(Guid guid)
    {
        var res = await group.GetByGuidAsync(guid);
        return Ok(res);
    }

    [HttpGet("location/{guid}")]
    public async Task<IActionResult> GetByLocationAsync(Guid guid)
    {
        var res = await group.GetByLocationAsync(guid);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGroupDto dto)
    {
        
    
        var res = await group.CreateAsync(dto);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateGroupDto dto)
    {
        

        var res = await group.UpdateAsync(dto);
        return Ok(res);
    }

    [HttpDelete("{guid}")]
    public async Task<IActionResult> DeleteAsync(Guid guid)
    {
        

        var res = await group.DeleteByGuidAsync(guid);
        return Ok(res);
    }

    [HttpDelete("list")]
    public async Task<IActionResult> DeleteListAsync([FromBody] List<Guid> guids)
    {
        

        var res = await group.DeleteListAsync(guids);
        return Ok(res);
    }

    [HttpPut("enable/{guid}")]
    public async Task<IActionResult> EnableAsync(Guid guid)
    {
        

        var res = await group.EnabledAsync(guid);
        return Ok(res);
    }

    [HttpPut("disable/{guid}")]
    public async Task<IActionResult> DisableAsync(Guid guid)
    {
        

        var res = await group.DisabledAsync(guid);
        return Ok(res);
    }

}