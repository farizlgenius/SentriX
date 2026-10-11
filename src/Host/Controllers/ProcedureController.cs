using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProcediureController(IProcedure proc) : ControllerBase
{
      
}