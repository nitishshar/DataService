using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using DataService.Helpers;
using DataService.Models;
using System;
namespace DataService.Domain
{
    public class PivotServiceApi
    {
        private const string tableName = "employeedetails";

        internal DBConnector Db { get; set; }
        private SQLBuilder queryBuilder { get; set; }
        public PivotServiceApi()
        {
        }

        internal PivotServiceApi(DBConnector db)
        {
            Db = db;
            queryBuilder = new SQLBuilder();
        }

        public async Task<GetRowsResponse> GetRows(Request request)
        {

            using var cmd = Db.Connection.CreateCommand();
            var pivotValues = new Dictionary<string, List<string>>();
            if (request.PivotMode)
            {
                string whereSQl = queryBuilder.WhereSQL(request, tableName);
                Console.WriteLine(whereSQl);
                pivotValues = await getPivotValues(request.PivotCols, whereSQl);
            }
            string sql = queryBuilder.CreateSQL(request, tableName, pivotValues);
            cmd.CommandText = sql;
            Console.WriteLine(request);
            Console.WriteLine("SQL" + sql);
            try
            {
                var response = await ListExtensions.ReadAsync(await cmd.ExecuteReaderAsync());
                return await Task.Run(() => ResponseBuilder.createResponse(request, response, pivotValues));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + sql);
                return null;
            }
        }
        private async Task<Dictionary<string, List<string>>> getPivotValues(List<ColumnVO> pivotCols, string where = "")
        {
            var pivotColumns = pivotCols.Select(column => column.Field).ToList();
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
            await pivotColumns.ForEachAsync(async column =>
             {
                 var values = await this.getPivotValues(column, where);
                 results.Add(column, values);

             });
            return results;
        }

        private async Task<List<string>> getPivotValues(string pivotColumn, string where = "")
        {
            string sql = string.Format("SELECT DISTINCT {0} FROM {1} {2}", pivotColumn, tableName, where);
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = sql;
            Console.WriteLine(sql);
            return await ListExtensions.ReadDistinctAsync(await cmd.ExecuteReaderAsync());
        }


    }
}
