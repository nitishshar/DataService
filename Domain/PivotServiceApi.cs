using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataService.Helpers;
using DataService.Models;
using System;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace DataService.Domain
{
    public class PivotServiceApi
    {
        private const string tableName = "employeedetails";

        internal DBConnector Db { get; set; }

        private IMemoryCache _memoryCache;

        private SQLBuilder queryBuilder { get; set; }
        public PivotServiceApi()
        {
        }

        internal PivotServiceApi(DBConnector db, IMemoryCache memoryCache)
        {
            Db = db;
            _memoryCache = memoryCache;
            queryBuilder = new SQLBuilder();
        }

        public async Task<GetRowsResponse> GetRows(Request request)
        {

            using var cmd = Db.Connection.CreateCommand();
            var pivotValues = new ConcurrentDictionary<string, List<string>>();
            if (request.PivotMode)
            {
                string whereSQl = queryBuilder.WhereSQL(request, tableName);
                Console.WriteLine(whereSQl);
                pivotValues = await getPivotValues(request.PivotCols, whereSQl);
            }
            Console.WriteLine(string.Join(",", pivotValues.Keys) + "test");
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
        private async Task<ConcurrentDictionary<string, List<string>>> getPivotValues(List<ColumnVO> pivotCols, string where = "")
        {
            var pivotColumns = pivotCols.Select(column => column.Field).ToList();
            ConcurrentDictionary<string, List<string>> results = new ConcurrentDictionary<string, List<string>>();
            await pivotColumns.ParallelForEachAsync((column) => this.getPivotValues(column, where).ContinueWith(res => results.GetOrAdd(column, res.Result)));
            return results;
        }

        private async Task<List<string>> getPivotValues(string pivotColumn, string where = "")
        {
            List<string> results = new List<string>();
            string sql = string.Format("SELECT DISTINCT {0} FROM {1} {2}", pivotColumn, tableName, where);
            if (!_memoryCache.TryGetValue(sql, out results))
            {
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = sql;
                Console.WriteLine(sql);
                results = await ListExtensions.ReadDistinctAsync(await cmd.ExecuteReaderAsync());
                Console.WriteLine(string.Join(",", results));
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(4));
                if (results.Count > 0)
                {
                    _memoryCache.Set(sql, results, cacheEntryOptions);
                }
            }
            return results;
        }
    }
}
