using System.Collections.Generic;
using System.Linq;
using DataService.Models;

namespace DataService.Helpers
{
    public class ResponseBuilder
    {
        public static GetRowsResponse createResponse(
            Request request,
            List<Dictionary<string, object>> rows,
            Dictionary<string, List<string>> pivotValues)
        {

            int currentLastRow = request.StartRow + rows.Count;
            int lastRow = currentLastRow <= request.EndRow ? currentLastRow : -1;

            List<ColumnVO> valueColumns = request.ValueCols;

            return new GetRowsResponse(rows, lastRow, getSecondaryColumns(pivotValues, valueColumns));
        }

        private static List<string> getSecondaryColumns(Dictionary<string, List<string>> pivotValues, List<ColumnVO> valueColumns)
        {
            List<IEnumerable<KeyValuePair<string, string>>> pivotPairs = pivotValues.Select(e => e.Value.ToList().Select(pivotValue => new KeyValuePair<string, string>(e.Key, pivotValue))).ToList();
   
            return pivotPairs.CartesianProduct().ToList().SelectMany(pairs =>  {                
                string pivotCol = string.Join("_", pairs.Select(pair => pair.Value));
                return valueColumns.Select(valueCol => pivotCol + "_" + valueCol.Field);
            }).ToList();
        }
    }
}