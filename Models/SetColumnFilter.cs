using System.Collections.Generic;

namespace DataService.Models
{
    public class SetColumnFilter : ColumnFilter
    {
        private List<string> values;

        public SetColumnFilter() { }

        public SetColumnFilter(List<string> values)
        {
            this.values = values;
        }

        public List<string> getValues()
        {
            return values;
        }
    }
}
