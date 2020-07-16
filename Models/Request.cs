using System.Collections.Generic;

namespace DataService.Models
{
    public class Request
    {

        private int startRow, endRow;

        // row group columns
        private List<ColumnVO> rowGroupCols;

        // value columns
        private List<ColumnVO> valueCols;

        // pivot columns
        private List<ColumnVO> pivotCols;

        // true if pivot mode is one, otherwise false
        private bool pivotMode;

        // what groups the user is viewing
        private List<string> groupKeys;

        // if filtering, what the filter model is
        private Dictionary<string, ColumnFilter> filterModel;

        // if sorting, what the sort model is
        private List<SortModel> sortModel;

        public Request()
        {
            this.rowGroupCols = new List<ColumnVO>();
            this.valueCols = new List<ColumnVO>();
            this.pivotCols = new List<ColumnVO>();
            this.groupKeys = new List<string>();
            this.filterModel = new Dictionary<string, ColumnFilter>();
            this.sortModel = new List<SortModel>();
        }

        public int getStartRow()
        {
            return startRow;
        }

        public void setStartRow(int startRow)
        {
            this.startRow = startRow;
        }

        public int getEndRow()
        {
            return endRow;
        }

        public void setEndRow(int endRow)
        {
            this.endRow = endRow;
        }

        public List<ColumnVO> getRowGroupCols()
        {
            return rowGroupCols;
        }

        public void setRowGroupCols(List<ColumnVO> rowGroupCols)
        {
            this.rowGroupCols = rowGroupCols;
        }

        public List<ColumnVO> getValueCols()
        {
            return valueCols;
        }

        public void setValueCols(List<ColumnVO> valueCols)
        {
            this.valueCols = valueCols;
        }

        public List<ColumnVO> getPivotCols()
        {
            return pivotCols;
        }

        public void setPivotCols(List<ColumnVO> pivotCols)
        {
            this.pivotCols = pivotCols;
        }

        public bool isPivotMode()
        {
            return pivotMode;
        }

        public void setPivotMode(bool pivotMode)
        {
            this.pivotMode = pivotMode;
        }

        public List<string> getGroupKeys()
        {
            return groupKeys;
        }

        public void setGroupKeys(List<string> groupKeys)
        {
            this.groupKeys = groupKeys;
        }

        public Dictionary<string, ColumnFilter> getFilterModel()
        {
            return filterModel;
        }

        public void setFilterModel(Dictionary<string, ColumnFilter> filterModel)
        {
            this.filterModel = filterModel;
        }

        public List<SortModel> getSortModel()
        {
            return sortModel;
        }

        public void setSortModel(List<SortModel> sortModel)
        {
            this.sortModel = sortModel;
        }
    }
}