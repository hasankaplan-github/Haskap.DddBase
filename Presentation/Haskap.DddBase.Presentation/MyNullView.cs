using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Haskap.DddBase.Presentation;

public class MyNullView : IView
{
    public string Path => string.Empty;
    public Task RenderAsync(ViewContext context) => Task.CompletedTask;
}
