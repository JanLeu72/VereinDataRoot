using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MitgliedRequest : MitgliedModel
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public List<TableFilter> Filters { get; set; }
        public List<TableSort> Sorting { get; set; }
    }
}
