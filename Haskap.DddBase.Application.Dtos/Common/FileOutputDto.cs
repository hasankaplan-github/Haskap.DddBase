namespace Haskap.DddBase.Application.Dtos.Common;

public class FileOutputDto
{
    public Guid? Id { get; set; }
    public string OriginalName { get; set; }
    public string NewName { get; set; }
    public string? Extension { get; set; }
}
