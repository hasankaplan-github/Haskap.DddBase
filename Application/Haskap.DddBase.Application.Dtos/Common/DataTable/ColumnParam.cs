namespace Haskap.DddBase.Application.Dtos.Common.DataTable;

public class ColumnParam
{
    public string Data { get; set; }
    public string Name { get; set; }
    public bool Searchable { get; set; }
    public bool Orderable { get; set; }
    public SearchParam Search { get; set; }
}
