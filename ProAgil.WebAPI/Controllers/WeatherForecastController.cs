using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProAgil.WebAPI.Models;

namespace ProAgil.WebAPI.Controllers
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

        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return new Event[] {
                new Event{
                    Id = 1,
                    Theme = "Angular e .NET Core",
                    Local = "Hortolandia",
                    Lot = "1º Lote",
                    PersonQtd = 250,
                    EventDate = DateTime.Now.AddDays(2).ToString("dd/MM/yyy")
                },
                new Event{
                    Id = 2,
                    Theme = "Angular",
                    Local = "São Paulo",
                    Lot = "2º Lote",
                    PersonQtd = 350,
                    EventDate = DateTime.Now.AddDays(2).ToString("dd/MM/yyy")
                },
                new Event{
                    Id = 3,
                    Theme = ".NET Core",
                    Local = "Campinas",
                    Lot = "3º Lote",
                    PersonQtd = 450,
                    EventDate = DateTime.Now.AddDays(2).ToString("dd/MM/yyy")
                }
            };
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id){
            var _event =  new Event[] {
                new Event{
                    Id = 1,
                    Theme = "Angular e .NET Core",
                    Local = "Hortolandia",
                    Lot = "1º Lote",
                    PersonQtd = 250,
                    EventDate = DateTime.Now.AddDays(2).ToString("dd/MM/yyy")
                },
                new Event{
                    Id = 2,
                    Theme = "Angular",
                    Local = "São Paulo",
                    Lot = "2º Lote",
                    PersonQtd = 350,
                    EventDate = DateTime.Now.AddDays(2).ToString("dd/MM/yyy")
                },
                new Event{
                    Id = 3,
                    Theme = ".NET Core",
                    Local = "Campinas",
                    Lot = "3º Lote",
                    PersonQtd = 450,
                    EventDate = DateTime.Now.AddDays(2).ToString("dd/MM/yyy")
                }
            }.FirstOrDefault( a => a.Id == id);

            return Ok(_event);
        }
    }
}
