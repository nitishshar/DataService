using System.Collections.Generic;

namespace DataService.Models
{
    public class FilterRequest
    {

        private Dictionary<string, ColumnFilter> filterModel;

        public FilterRequest() { }

        public FilterRequest(Dictionary<string, ColumnFilter> filterModel)
        {
            this.filterModel = filterModel;
        }
        public Dictionary<string, ColumnFilter> FilterModel
        {
            get { return this.filterModel; }
            set { this.filterModel = value; }
        }

        public override string ToString()
        {
            return "FilterRequest{" +
                "filterModel=" + filterModel +
                '}';
        }
    }
}