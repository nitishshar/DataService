namespace DataService.Models
{
    public class NumberColumnFilter : ColumnFilter
    {
        private string type;
        private int filter;
        private int filterTo;

        public NumberColumnFilter() { }

        public NumberColumnFilter(string type, int filter, int filterTo)
        {
            this.type = type;
            this.filter = filter;
            this.filterTo = filterTo;
        }

        public string getFilterType()
        {
            return this.filterType;
        }

        public string getType()
        {
            return type;
        }

        public int getFilter()
        {
            return filter;
        }

        public int getFilterTo()
        {
            return filterTo;
        }
    }
}