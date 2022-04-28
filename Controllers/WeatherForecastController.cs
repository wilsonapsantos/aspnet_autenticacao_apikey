using aspnet_autenticacao_apikey.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_autenticacao_apikey.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet("")]
    [ApiKey]
    public IActionResult Get()
    {
        return Ok(new { message = "Você tem acesso!" });
    }
}
