using Haskap.DddBase.Application.Contracts;

namespace Modules.CustomMessage.Application.Contracts;

public interface ICustomMessageService : IUseCaseService
{
    void ShowMessage(Domain.CustomMessageAggregate.CustomMessage customMessage);
    string GetAllMessagesAsJson();
}