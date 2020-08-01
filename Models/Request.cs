using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DataService.Models
{
    public class Request
    {

        private int startRow;
        private int endRow;
        // row group columns
        private List<ColumnVO> rowGroupCols;

        // value columns
        private List<ColumnVO> valueCols;

        // pivot columns
        private List<ColumnVO> pivotCols;

        // true if pivot mode is one, otherwise false
        private bool pivotMode;

        // what groups the user is viewing
        private List<string> groupKeys{ get; set; }

        // if filtering, what the filter model is
        private ConcurrentDictionary<string, ColumnFilter> filterModel{ get; set; }

        // if sorting, what the sort model is
        private List<SortModel> sortModel{ get; set; }

        public Request()
        {
            this.rowGroupCols = new List<ColumnVO>();
            this.valueCols = new List<ColumnVO>();
            this.pivotCols = new List<ColumnVO>();
            this.groupKeys = new List<string>();
            this.filterModel = new ConcurrentDictionary<string, ColumnFilter>();
            this.sortModel = new List<SortModel>();
        }

        public int StartRow
        {
            get { return this.startRow; }
            set { this.startRow = value; }
        }     

        public int EndRow
        {
            get { return this.endRow; }
            set { this.endRow = value; }
        }

        public List<ColumnVO> RowGroupCols
        {
            get { return this.rowGroupCols; }
            set { this.rowGroupCols = value; }
        }  

        public List<ColumnVO> ValueCols
        {
           get { return this.valueCols; }
            set { this.valueCols = value; }
        }  
        public List<ColumnVO> PivotCols
        {
            get { return this.pivotCols; }
            set { this.pivotCols = value; }
        }    

        public bool PivotMode
        {
            get { return this.pivotMode; }
            set { this.pivotMode = value; }
        }
    
        public List<string> GroupKeys
        {
           get { return this.groupKeys; }
            set { this.groupKeys = value; }
        }       

        public ConcurrentDictionary<string, ColumnFilter> FilterModel
        {
            get { return this.filterModel; }
            set { this.filterModel = value; }
        }


        public List<SortModel> SortModel
        {
             get { return this.sortModel; }
            set { this.sortModel = value; }
        }

    }
}