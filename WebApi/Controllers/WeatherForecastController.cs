using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static bool Disabled = false;

		[HttpGet(Name = "GetWeatherForecast")]
        [Authorize(Roles = "weather.read")]
        public IActionResult Get()
		{
			if(Disabled)
			{
				return StatusCode(503);
			}

			return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
			})
			.ToArray());
		}

		[HttpPut(Name = "Disable")]
        [Authorize(Roles = "weather.update")]
        public IActionResult Disable(bool disabled)
		{
			Disabled = disabled;
			return Ok();
		}
	}
}