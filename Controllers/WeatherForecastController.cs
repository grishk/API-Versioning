using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_Versioning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiVersion("2.0")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IList<WeatherForecast> _forecasts;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            string[] summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            _forecasts = GetRandomForecast(summaries);
        }

        private IList<WeatherForecast> GetRandomForecast(string[] summaries)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = summaries[rng.Next(summaries.Length)]
                })
                .ToList();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _forecasts;
        }

        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        public WeatherForecast Get10(int id)
        {
            return _forecasts[id];
        }

        [HttpGet("{id}")]
        [MapToApiVersion("1.1")]
        public WeatherForecast Get11(int id)
        {
            _forecasts[id].ExtraData = "extra";
            return _forecasts[id];
        }

        [HttpGet("{id}")]
        [MapToApiVersion("2.0")]
        public WeatherForecast Get12(int id)
        {
            _forecasts[id].ExtraData = "extra";
            return _forecasts[id];
        }

        [HttpGet("{id}")]
        [MapToApiVersion("2.0")]
        [MapToApiVersion("3.0")]
        public WeatherForecast Get3(int id)
        {
            _forecasts[id].ExtraData = "extra";
            return _forecasts[id];
        }
    }
}
