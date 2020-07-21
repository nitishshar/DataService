using System;

namespace DataService.Models
{
    public class SortModel
    {

        private string colId;
        private string sort;

        public SortModel() { }

        public SortModel(string colId, string sort)
        {
            this.colId = colId;
            this.sort = sort;
        }


        public string ColId
        {
            get { return this.colId; }
            set { this.colId = value; }
        }


        public string Sort
        {
            get { return this.sort; }
            set { this.sort = value; }
        }

        public override bool Equals(object obj)
        {
            return obj is SortModel model &&
                   colId == model.colId &&
                   sort == model.sort;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(colId, sort);
        }

        public override string ToString()
        {
            return "SortModel{" +
                    "colId='" + colId + '\'' +
                    ", sort='" + sort + '\'' +
                    '}';
        }
    }
}