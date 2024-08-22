namespace Backstage.Models
{
    public class DataTablesRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
        public string typeFilter { get; set; }
        public int? pageSize { get; set; }
    }
}
