using Haskap.DddBase.Application;
using Modules.CustomMessage.Application.Contracts;
using Modules.CustomMessage.Application.Dtos;
using Modules.CustomMessage.Application.Mappings;
using System.Text.Json;

namespace Modules.CustomMessage.Application;

public class CustomMessageService : UseCaseService, ICustomMessageService
{
    private readonly Lazy<List<CustomMessageDto>> _customMessages = new();

    public void ShowSuccessMessage(string? title, string? text)
    {
        var customMessage = Domain.CustomMessageAggregate.CustomMessage.Success(title, text);
        _customMessages.Value.Add(customMessage.ToCustomMessageDto());
    }

    public void ShowErrorMessage(string? title, string? text)
    {
        var customMessage = Domain.CustomMessageAggregate.CustomMessage.Error(title, text);
        _customMessages.Value.Add(customMessage.ToCustomMessageDto());
    }

    public void ShowWarningMessage(string? title, string? text)
    {
        var customMessage = Domain.CustomMessageAggregate.CustomMessage.Warning(title, text);
        _customMessages.Value.Add(customMessage.ToCustomMessageDto());
    }

    public void ShowInfoMessage(string? title, string? text)
    {
        var customMessage = Domain.CustomMessageAggregate.CustomMessage.Info(title, text);
        _customMessages.Value.Add(customMessage.ToCustomMessageDto());
    }

    public void ShowQuestionMessage(string? title, string? text)
    {
        var customMessage = Domain.CustomMessageAggregate.CustomMessage.Question(title, text);
        _customMessages.Value.Add(customMessage.ToCustomMessageDto());
    }

    public string GetAllMessagesAsJson()
    {
        return JsonSerializer.Serialize(_customMessages.Value);
    }
}