using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain.Entities;
using ProAgil.Repository.Data;

namespace ProAgil.WebAPI.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class EventController : ControllerBase {
        private readonly IProAgilRepository context;
        public EventController (IProAgilRepository context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get () {
            try {
                var events = await context.GetEventsAssync (true);
                return Ok (events);
            } catch (Exception) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, "Não é possível obtér a lista de eventos");
            }
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> Get (int id) {
            try {
                var _event = await context.GetEventAssyncById (id, true);
                return Ok (_event);
            } catch (System.Exception) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, "Não é possível obtér o evento");
            }
        }

        /// <summary>
        /// Eventos por tema
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        [HttpGet ("getbytheme/{id}")]
        public async Task<IActionResult> GetEventsByTheme (string theme) {
            try {
                var _event = await context.GetEventsAssyncByTheme (theme, true);
                return Ok (_event);
            } catch (Exception) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, "Não é possível obtér o evento");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post (Event model) {
            try {
                context.Add (model);
                if (await context.SaveChangeAssync ())
                    return Created ($"/event/{model.Id}", model);
                else
                    BadRequest ();
                return Ok ();
            } catch (Exception) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, "Não é possível criar o evento");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put (Event model) {
            try {
                var _event = await context.GetEventAssyncById (model.Id, false);

                if (_event == null) return NotFound ();

                context.Update (model);
                if (await context.SaveChangeAssync ())
                    return Created ($"/event/{model.Id}", model);
                else
                    return BadRequest ();
            } catch (Exception) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, "Não é possível alterar o evento");
            }
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete (int id) {
            try {
                var _event = await context.GetEventAssyncById (id, false);

                if (_event == null) return NotFound ();

                context.Delete (_event);
                if (await context.SaveChangeAssync ())
                    return Ok ();
                else
                    return BadRequest ();
            } catch (Exception) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, "Não é possível alterar o evento");
            }
        }
    }
}