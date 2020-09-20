using System.Net.Http.Headers;
using System.IO;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain.Entities;
using ProAgil.Repository.Data;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Controllers 
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IProAgilRepository context;
        private readonly IMapper mapper;
        public EventController(IProAgilRepository context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var events = await context.GetEventsAssync(true);
                var results = mapper.Map<EventDto[]>(events);
                return Ok(results);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Não é possível obtér a lista de eventos: {e.Message}");
            }
        }

        [HttpPost("upload")]
        public IActionResult upload()
        {
            try
            {
                // pega o arquivo
                var file = Request.Form.Files[0];
                // pega o diretório onde a aplicação quer armazenar
                var folferName = Path.Combine("Resources","Images");
                // Combina o diretório da aplicação + o onde a aplicação quer armazenar os arquivos
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folferName);

                if (file.Length > 0)
                {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        // copia file parao stream
                        file.CopyTo(stream);
                    }

                    return Ok();
                }
                return BadRequest("Erro ao fazer upload");
            }
            
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Não é possível obtér a lista de eventos: {e.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get (int id) {
            try {
                var _event = await context.GetEventAssyncById (id, true);
                var result = mapper.Map<EventDto>(_event);
                return Ok (result);
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
                var _events = await context.GetEventsAssyncByTheme (theme, true);
                var results = mapper.Map<EventDto[]>(_events);
                return Ok (results);
            } catch (Exception e) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, $"Não é possível obtér o evento: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post (EventDto model) {
            try {
                var _event = mapper.Map<Event>(model);
                context.Add (_event);
                if (await context.SaveChangeAssync ())
                    return Created ($"/event/{_event.Id}", mapper.Map<EventDto>(_event));
                else
                    BadRequest ();
                return Ok ();
            } catch (Exception e) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, $"Não é possível criar o evento: {e.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put (int id, EventDto model) {
            try {
                if (id != model.Id)
                     return BadRequest ();

                var _event = await context.GetEventAssyncById (id, false);

                if (_event is null) return NotFound ();

                mapper.Map(model, _event);

                context.Update (_event);
                if (await context.SaveChangeAssync ())
                    return Created ($"/event/{_event.Id}",  mapper.Map<EventDto>(_event));
                else
                    return BadRequest ();
            } catch (Exception e) {
                return this.StatusCode (StatusCodes.Status500InternalServerError, $"Não é possível alterar o evento {e.Message}");
            }
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete ([FromRoute] int id) {
            try {
                var _event = await context.GetEventAssyncById (id, false);

                if (_event is null) return NotFound ();

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