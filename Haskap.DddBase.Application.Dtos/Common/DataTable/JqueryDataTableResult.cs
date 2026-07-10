namespace Haskap.DddBase.Application.Dtos.Common.DataTable;

public class JqueryDataTableResult
{
    // properties are not capital due to json mapping
    public int draw { get; set; }
    public int recordsTotal { get; set; }
    public int recordsFiltered { get; set; }
    public dynamic data { get; set; }
}
