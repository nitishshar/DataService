using System.Collections.Generic;

namespace DataService.Models
{
    public class ColumnFilter
    {
        private string filterType;
        public ColumnFilter()
        {

        }
        public ColumnFilter(string filterType)
        {
            this.filterType = filterType;
        }

        public string FilterType
        {
            get { return this.filterType; }
            set { this.filterType = value; }
        }

    }
}