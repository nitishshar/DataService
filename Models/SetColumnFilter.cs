using System.Collections.Generic;

namespace DataService.Models
{
    public class SetColumnFilter : ColumnFilter
    {
        private List<string> values;

        public SetColumnFilter() { }
        public SetColumnFilter(List<string> values, string filterType) : base(filterType)
        {
            this.values = values;
        }

        public List<string> Values
        {
            get { return this.values; }
            set { this.values = value; }
        }
    }
}
