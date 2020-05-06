using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProAgil.WebAPI.Data;
using ProAgil.WebAPI.Models;

namespace ProAgil.WebAPI.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class EventController : ControllerBase {
        private readonly DataContext context;
        public EventController (DataContext context) {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Event> Get () {
            var events = context.Events.ToList ();
            return events;
        }

        [HttpGet ("{id}")]
        public IActionResult Get (int id) {
            var _event = new Event[] {
                new Event {
                    Id = 1,
                        Theme = "Angular e .NET Core",
                        Local = "Hortolandia",
                        Lot = "1º Lote",
                        PersonQtd = 250,
                        EventDate = DateTime.Now.AddDays (2).ToString ("dd/MM/yyy")
                },
                new Event {
                    Id = 2,
                        Theme = "Angular",
                        Local = "São Paulo",
                        Lot = "2º Lote",
                        PersonQtd = 350,
                        EventDate = DateTime.Now.AddDays (2).ToString ("dd/MM/yyy")
                },
                new Event {
                    Id = 3,
                        Theme = ".NET Core",
                        Local = "Campinas",
                        Lot = "3º Lote",
                        PersonQtd = 450,
                        EventDate = DateTime.Now.AddDays (2).ToString ("dd/MM/yyy")
                }
            }.FirstOrDefault (a => a.Id == id);

            return Ok (_event);
        }
    }
}