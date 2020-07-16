using System.Collections.Generic;

namespace DataService.Models
{
    public class EnterpriseGetRowsResponse
    {
        private List<Dictionary<string, object>> data;
        private int lastRow;
        private List<string> secondaryColumnFields;

        public EnterpriseGetRowsResponse() { }

        public EnterpriseGetRowsResponse(List<Dictionary<string, object>> data, int lastRow, List<string> secondaryColumnFields)
        {
            this.data = data;
            this.lastRow = lastRow;
            this.secondaryColumnFields = secondaryColumnFields;
        }

        public List<Dictionary<string, object>> getData()
        {
            return data;
        }

        public void setData(List<Dictionary<string, object>> data)
        {
            this.data = data;
        }

        public int getLastRow()
        {
            return lastRow;
        }

        public void setLastRow(int lastRow)
        {
            this.lastRow = lastRow;
        }

        public List<string> getSecondaryColumnFields()
        {
            return secondaryColumnFields;
        }

        public void setSecondaryColumns(List<string> secondaryColumnFields)
        {
            this.secondaryColumnFields = secondaryColumnFields;
        }
    }
}