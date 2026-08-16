using Core.Contract.DTOs.License;
using Core.Contract.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LicenseController(ILicense lic) : ControllerBase
{

}