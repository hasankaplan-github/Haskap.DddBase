using Modules.CustomMessage.Domain.Shared.Consts;

namespace Modules.CustomMessage.Domain.CustomMessageAggregate;

public class CustomMessage
{
    public string Icon { get; private set; }
    public string? Title { get; private set; }
    public string? Text { get; private set; }

    private CustomMessage()
    { }

    private CustomMessage(string icon, string? title, string? text)
    {
        Icon = icon;
        Title = title;
        Text = text;
    }

    public static CustomMessage Success(string? title, string? text)
    {
        return new(CustomMessageConsts.SuccessIcon, title, text);
    }

    public static CustomMessage Error(string? title, string? text)
    {
        return new(CustomMessageConsts.ErrorIcon, title, text);
    }

    public static CustomMessage Warning(string? title, string? text)
    {
        return new(CustomMessageConsts.WarningIcon, title, text);
    }

    public static CustomMessage Info(string? title, string? text)
    {
        return new(CustomMessageConsts.InfoIcon, title, text);
    }

    public static CustomMessage Question(string? title, string? text)
    {
        return new(CustomMessageConsts.QuestionIcon, title, text);
    }
}