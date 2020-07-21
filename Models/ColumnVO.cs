using System;

namespace DataService.Models
{
    public class ColumnVO
    {
        private string id;
        private string displayName;
        private string field;
        private string aggFunc;

        public ColumnVO()
        {
        }

        public ColumnVO(string id, string displayName, string field, string aggFunc)
        {
            this.id = id;
            this.displayName = displayName;
            this.field = field;
            this.aggFunc = aggFunc;
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string DisplayName
        {
            get { return this.displayName; }
            set { this.displayName = value; }
        }

        public string Field
        {
            get { return this.field; }
            set { this.field = value; }
        }


        public string AggFunc
        {
            get { return this.aggFunc; }
            set { this.aggFunc = value; }
        }




        public override bool Equals(object obj)
        {
            return obj is ColumnVO vO &&
                   id == vO.id &&
                   displayName == vO.displayName &&
                   field == vO.field &&
                   aggFunc == vO.aggFunc;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, displayName, field, aggFunc);
        }

    }
}