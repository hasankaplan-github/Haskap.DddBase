namespace Haskap.DddBase.Application.Dtos.Common;

public class FileInputDto
{
    public long ContentLength { get; set; }
    public byte[] Content { get; set; }

    public string OriginalName { get; set; }
    public string NewName { get; set; }
    public string? Extension { get; set; }
}
