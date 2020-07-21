using System;
using DataService.Models;

namespace DataService.Helpers
{
    public class ColumnFilterJsonConverter : DerivedTypeJsonConverter<ColumnFilter>
    {
        protected override Type NameToType(string typeName)
        {
            return typeName switch
            {
                // map string values to types
                nameof(SetColumnFilter) => typeof(SetColumnFilter),
                nameof(NumberColumnFilter) => typeof(NumberColumnFilter)
                
                // TODO: Create a case for each derived type
            };
        }

        protected override string TypeToName(Type type)
        {
            // map types to string values
            if (type == typeof(SetColumnFilter)) return nameof(SetColumnFilter);
            if (type == typeof(NumberColumnFilter)) return nameof(NumberColumnFilter);
            return "";
            // TODO: Create a condition for each derived type
        }
    }
}