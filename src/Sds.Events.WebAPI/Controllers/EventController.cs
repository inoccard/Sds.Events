using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sds.Events.Domain.Core;
using Sds.Events.Domain.Entities;
using Sds.Events.Repository.Data;
using Sds.Events.WebAPI.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sds.Events.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/event")]
    public class EventController : MainController
    {
        private readonly IEventsRepository _context;
        private readonly IMapper _mapper;

        public EventController(IEventsRepository context, IMapper mapper, INotifierMessage notifier)
            : base(notifier)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna todos os eventos
        /// </summary>
        /// <returns></returns>
        [HttpGet("events")]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var events = await _context.GetEventsAssync(true);
                if (events.Length == 0)
                {
                    AddMessage("Nenhum evento encontrado");
                    return CustomResponse(statusCode: 404);
                }

                var results = _mapper.Map<EventDto[]>(events.OrderBy(e => e.Id));
                return CustomResponse(results);
            }
            catch (Exception e)
            {
                return HandleException($"Não é possível obtér a lista de eventos: {e.Message}");
            }
        }

        /// <summary>
        /// Retorna um evento específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            try
            {
                var _event = await _context.GetEventAssyncById(id, true);
                if (_event == null)
                {
                    AddMessage("Evento não encontrado!");
                    return CustomResponse(statusCode: 404);
                }
                var result = _mapper.Map<EventDto>(_event);
                return CustomResponse(result);
            }
            catch (Exception)
            {
                return HandleException("Não é possível obter o evento, verifique o id informado e tente novamente");
            }
        }

        /// <summary>
        /// Retorna eventos por tema
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        [HttpGet("get-by-theme")]
        public async Task<IActionResult> GetEventsByTheme([FromQuery] string theme)
        {
            try
            {
                var _events = await _context.GetEventsAssyncByTheme(theme, true);
                if (_events == null)
                {
                    AddMessage("Nenhum evento encontrado");
                    return CustomResponse(statusCode: 404);
                }
                var results = _mapper.Map<EventDto[]>(_events);
                return CustomResponse(results);
            }
            catch (Exception e)
            {
                return HandleException($"Não é possível obtér o evento: {e.Message}");
            }
        }

        /// <summary>
        /// Cria um evento
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CustomResponse(ModelState);

                var _event = _mapper.Map<Event>(model);
                _context.Add(_event);

                if (!await _context.SaveChangeAssync())
                {
                    AddMessage("Não foi possível salvar o evento, verifique os dados e tente novamente!");
                    return CustomResponse();
                }

                return CustomResponse(_mapper.Map<EventDto>(_event), 201, $"/event/{_event.Id}");
            }
            catch (Exception e)
            {
                return HandleException($"Não é possível criar o evento: {e.Message}");
            }
        }

        /// <summary>
        /// Atualiza um evento
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto model)
        {
            try
            {
                if (id != model.Id)
                {
                    AddMessage("Os ids fornecidos na rota e no objeto são diferentes");
                    return CustomResponse();
                }
                var lotIds = AddLotIds(model);
                var socialIds = AddSocialNetworksIds(model);

                var _event = await _context.GetEventAssyncById(id, false);

                if (_event is null)
                {
                    AddMessage("Não é possível atualizar o evento: Evento não encontrado!");
                    return CustomResponse(statusCode: 404);
                }

                var lots = _event.Lots.Where(lot => !lotIds.Contains(lot.Id)).ToArray();

                if (lots.Length > 0)
                    _context.DeleteRange(lots);

                var socialNetworks = _event.SocialNetworks.Where(social => !socialIds.Contains(social.Id)).ToArray();

                if (socialNetworks.Length > 0)
                    _context.DeleteRange(socialNetworks);

                _mapper.Map(model, _event);

                _context.Update(_event);
                if (!await _context.SaveChangeAssync())
                    return CustomResponse();

                return CustomResponse(_mapper.Map<EventDto>(_event), 201, $"/event/{_event.Id}");
            }
            catch (Exception e)
            {
                return HandleException($"Não é possível atualizar o evento: {e.Message}");
            }
        }

        /// <summary>
        /// Deleta uum evento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            try
            {
                var _event = await _context.GetEventAssyncById(id, false);

                if (_event is null)
                {
                    AddMessage("Não é possível deletar o evento: Evento não encontrado!");
                    return CustomResponse(statusCode: 404);
                }

                _context.Delete(_event);
                if (!await _context.SaveChangeAssync())
                    return CustomResponse();

                return CustomResponse();
            }
            catch (Exception e)
            {
                return HandleException($"Não é possível deletar o evento: {e.Message}");
            }
        }

        /// <summary>
        /// Salva uma imagem
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        public IActionResult Upload()
        {
            try
            {
                // pega o arquivo
                var file = Request.Form.Files[0];
                // pega o diretório onde a aplicação quer armazenar
                var folferName = Path.Combine("Resources", "Images");
                // Combina o diretório da aplicação + o onde a aplicação quer armazenar os arquivos
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folferName);

                if (file.Length == 0)
                {
                    AddMessage("Erro ao fazer upload");
                    return CustomResponse();
                }

                var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    // copia file parao stream
                    file.CopyTo(stream);
                }

                return CustomResponse();
            }
            catch (Exception e)
            {
                return HandleException($"Não é possível obtér a lista de eventos: {e.Message}");
            }
        }

        #region Private Methods

        private static List<int> AddSocialNetworksIds(EventDto model)
        {
            List<int> socialIds = new();
            if (model.SocialNetworks != null && model.SocialNetworks.Count > 0)
                socialIds.AddRange(model.SocialNetworks.Select(social => social.Id));

            return socialIds;
        }

        private static List<int> AddLotIds(EventDto model)
        {
            List<int> lotIds = new();
            if (model.Lots != null && model.Lots.Count > 0)
                lotIds.AddRange(model.Lots.Select(lot => lot.Id));

            return lotIds;
        }

        #endregion Private Methods
    }
}