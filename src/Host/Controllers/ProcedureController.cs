using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProcedureController(IProcedure proc) : ControllerBase
{
      
}