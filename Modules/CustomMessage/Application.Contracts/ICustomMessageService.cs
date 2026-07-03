using Haskap.DddBase.Application.Contracts;

namespace Modules.CustomMessage.Application.Contracts;

public interface ICustomMessageService : IUseCaseService
{
    void ShowSuccessMessage(string? title, string? text);
    void ShowErrorMessage(string? title, string? text);
    void ShowWarningMessage(string? title, string? text);
    void ShowInfoMessage(string? title, string? text);
    void ShowQuestionMessage(string? title, string? text);
    string GetAllMessagesAsJson();
}