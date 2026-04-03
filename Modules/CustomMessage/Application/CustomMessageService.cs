using Haskap.DddBase.Application;
using Modules.CustomMessage.Application.Contracts;
using Modules.CustomMessage.Application.Dtos;
using Modules.CustomMessage.Application.Mappings;
using System.Text.Json;

namespace Modules.CustomMessage.Application;

public class CustomMessageService : UseCaseService, ICustomMessageService
{
    private readonly Lazy<List<CustomMessageDto>> _customMessages = new();

    public void ShowMessage(Domain.CustomMessageAggregate.CustomMessage customMessage)
    {
        _customMessages.Value.Add(customMessage.ToCustomMessageDto());
    }

    public string GetAllMessagesAsJson()
    {
        return JsonSerializer.Serialize(_customMessages.Value);
    }
}