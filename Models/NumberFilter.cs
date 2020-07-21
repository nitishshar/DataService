namespace DataService.Models
{
    public class NumberColumnFilter : ColumnFilter
    {
        private string type;
        private int filter;
        private int filterTo;

        public NumberColumnFilter() { }

        public NumberColumnFilter(string type, int filter, int filterTo, string filterType) : base(filterType)
        {
            this.type = type;
            this.filter = filter;
            this.filterTo = filterTo;
        }
        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        public int Filter
        {
            get { return this.filter; }
            set { this.filter = value; }
        }
        public int FilterTo
        {
            get { return this.filterTo; }
            set { this.filterTo = value; }
        }

    }
}