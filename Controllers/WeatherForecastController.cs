using api_login.CustomExceptions;
using api_login.Interfaces;
using api_login.Model;
using Microsoft.AspNetCore.Mvc;

namespace api_login.Controllers
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
        private readonly IBasicValidations _basicValidations;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                         IBasicValidations basicValidations)
        {
            _logger = logger;
            _basicValidations = basicValidations;
        }

        //[Authorize] // verificação básica de autenticação e autorização do framework e resposta de forma padrão
        [HttpGet("Get")]
        public IEnumerable<WeatherForecast> Get()
        {
            if (!_basicValidations.IsAuthenticatedUser())
            {
                throw new UnAuthenticatedAccessException("Essa mensagem vai ser substituída no CustomExceptionFilter.");
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }

        [HttpGet("GetAdministrator")]
        public IEnumerable<WeatherForecast> GetAdministrator()
        {
            var validationResult = _basicValidations.IsAdministratorUser();

            if (!validationResult.Authenticated)
            {
                throw new UnAuthenticatedAccessException("Essa mensagem vai ser substituída no CustomExceptionFilter.");
            }

            if (!validationResult.Administrator)
            {
                throw new UnauthorizedAccessException("Essa mensagem vai ser substituída no CustomExceptionFilter.");
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }
    }
}
