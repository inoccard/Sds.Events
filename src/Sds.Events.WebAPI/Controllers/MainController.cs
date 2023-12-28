using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sds.Events.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sds.Events.WebAPI.Controllers;

[Authorize]
[ApiController]
public abstract class MainController : ControllerBase
{
    public readonly INotifierMessage _notifierMessage;

    protected MainController(INotifierMessage notifierMessage)
    {
        _notifierMessage = notifierMessage;
    }

    protected ActionResult CustomResponse(object result = null, short statusCode = 400, string uri = null)
    {
        if (_notifierMessage.IsValid())
            return statusCode is 201 ? Created(uri, result) : Ok(result);

        return statusCode switch
        {
            401 => Unauthorized(InstatiateValidationProblem(statusCode)),
            404 => NotFound(InstatiateValidationProblem(statusCode)),
            409 => Conflict(InstatiateValidationProblem(statusCode)),
            _ => BadRequest(InstatiateValidationProblem(statusCode)),
        };
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        _notifierMessage.AddRange(erros.Select(e => e.ErrorMessage).ToArray());
        return CustomResponse();
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        _notifierMessage.Add(validationResult.ErrorMessage);
        return CustomResponse();
    }

    #region Notification

    protected void AddMessage(string message) => _notifierMessage.Add(message);
    protected void AddMessageRange(string[] message) => _notifierMessage.AddRange(message);

    protected string[] GetMessages() => _notifierMessage.GetMessages();

    protected bool IsValid() => _notifierMessage.IsValid();

    #endregion Notification

    private ResponseResult InstatiateValidationProblem(short statusCode)
        => new("Ocorreu um ou mais erros", statusCode, _notifierMessage.GetMessages());

    protected IActionResult HandleException(string errorMessage)
    {
        AddMessage(errorMessage);
        return CustomResponse();
    }

}