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
            try
            {
                var _event = context.Events.FirstOrDefault (a => a.Id == id);
            return Ok (_event);
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}