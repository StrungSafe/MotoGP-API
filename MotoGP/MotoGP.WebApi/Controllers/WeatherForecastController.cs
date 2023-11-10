using Microsoft.AspNetCore.Mvc;

namespace MotoGP.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public Task TempGet()
        {
            // an example for testing swagger docs
            return Task.CompletedTask;
        }
    }
}