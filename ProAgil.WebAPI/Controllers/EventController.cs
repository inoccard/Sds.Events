using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.WebAPI.Data;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class EventController : ControllerBase {
        private readonly DataContext context;
        public EventController (DataContext context) {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get () {
            try {
                var events = await context.Events.ToListAsync ();
                return Ok (events);
            } catch (System.Exception) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, "Não é possível obtér a lista de eventos");
            }
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> Get (int id) {
            try {
                var _event = await context.Events.FirstOrDefaultAsync (a => a.Id == id);
                return Ok (_event);
            } catch (System.Exception) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, "Não é possível obtér o evento");
            }
        }
    }
}