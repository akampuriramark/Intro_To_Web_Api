using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intro_To_Web_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private static readonly Random rng = new Random();
        private List<WeatherForecast> _weatherForecasts;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToList();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherForecasts;
        }

        [HttpGet("{id}")]
        public IEnumerable<WeatherForecast> Get(int id)
        {
            return _weatherForecasts.Where(x => x.Id.Equals(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] WeatherForecast weatherForecast)
        {
            // validate the object and its properties
            if (weatherForecast is null)
            {
                return BadRequest();
            }

            var rng = new Random();
            // generate Id to identify the forecast
            weatherForecast.Id = _weatherForecasts.Count;
            weatherForecast.Summary = Summaries[rng.Next(Summaries.Length)];
            return Ok(weatherForecast);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // check if id is valid
            if (id <= 0)
            {
                return BadRequest();
            }
            // find the forecast with id
            var forecast = _weatherForecasts.Where(x => x.Id.Equals(id));

            // object does not exist, return not found 
            if (forecast is null)
            {
                return NotFound();
            }

            // remove it from list
            _weatherForecasts = _weatherForecasts.Where(x => !x.Id.Equals(id)).ToList();

            return Ok(_weatherForecasts);
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] WeatherForecast weatherForecast)
        {
            // validate the object and its properties
            if (weatherForecast is null || weatherForecast.Id < 0)
            {
                return BadRequest();
            }

            var forecastToPatch = _weatherForecasts.Where(x => x.Id.Equals(weatherForecast.Id));

            // confirm the object to update exists
            if (forecastToPatch is null)
            {
                return NotFound();
            }

            // update object
            _weatherForecasts = _weatherForecasts.Where(x => x.Id.Equals(weatherForecast.Id))
                .Select(f => { f.TemperatureC = weatherForecast.TemperatureC;
                               f.Summary = Summaries[rng.Next(Summaries.Length)]; return f; })
                .ToList();

            // return updated object
            return Ok(_weatherForecasts.Where(x => x.Id.Equals(weatherForecast.Id)));
        }

    }
}
