namespace Models
{
    using System.Collections.Generic;
    public class GridFilters
    {
        public List<TableFilter> Filters { get; set; }
        public string Logic { get; set; }
    }
}
