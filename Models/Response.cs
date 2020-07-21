using System.Collections.Generic;

namespace DataService.Models
{
    public class GetRowsResponse
    {
        private List<Dictionary<string, object>> data;
        private int lastRow;
        private List<string> secondaryColumnFields;

        public GetRowsResponse() { }

        public GetRowsResponse(List<Dictionary<string, object>> data, int lastRow, List<string> secondaryColumnFields)
        {
            this.data = data;
            this.lastRow = lastRow;
            this.secondaryColumnFields = secondaryColumnFields;
        }

        public List<Dictionary<string, object>> Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
        public int LastRow
        {
            get { return this.lastRow; }
            set { this.lastRow = value; }
        }

        public List<string> SecondaryColumnFields
        {
            get { return this.secondaryColumnFields; }
            set { this.secondaryColumnFields = value; }
        }
    }
}