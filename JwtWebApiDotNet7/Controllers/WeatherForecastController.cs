using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
/*
    dotnet user-jwts create  ID: e2b29d17
    dotnet user-jwts key     Signing key: 2oWlQ/XPQ6m2QgHuniKbAOD7pOVsthkAtA3hGmn1KLc=
                            Token: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InJvYmVyIiwic3ViIjoicm9iZXIiLCJqdGkiOiJlMmIyOWQxNyIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjM2MzI0IiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMzAiLCJodHRwOi8vbG9jYWxob3N0OjUxMDgiLCJodHRwczovL2xvY2FsaG9zdDo3MDY0Il0sIm5iZiI6MTY4MjAwMDk4OSwiZXhwIjoxNjg5ODYzMzg5LCJpYXQiOjE2ODIwMDA5ODksImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.hn1FxApnnTsKkRxssAD542lUE17sfWvT8QQed7sVp9Y
    dotnet user-secrets list 

 */

namespace JwtWebApiDotNet7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast"), Authorize(Roles = "Admin,User")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}