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

        public string getId()
        {
            return id;
        }

        public void setId(string id)
        {
            this.id = id;
        }

        public string getDisplayName()
        {
            return displayName;
        }

        public void setDisplayName(string displayName)
        {
            this.displayName = displayName;
        }

        public string getField()
        {
            return field;
        }

        public void setField(string field)
        {
            this.field = field;
        }

        public string getAggFunc()
        {
            return aggFunc;
        }

        public void setAggFunc(string aggFunc)
        {
            this.aggFunc = aggFunc;
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