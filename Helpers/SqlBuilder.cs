using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DataService.Models;

namespace DataService.Helpers
{
    public class SQLBuilder
    {
        private List<string> groupKeys;
        private List<string> rowGroups;
        private List<string> rowGroupsToInclude;
        private bool isGrouping;
        private List<ColumnVO> valueColumns;
        private List<ColumnVO> pivotColumns;
        private ConcurrentDictionary<string, ColumnFilter> filterModel;
        private List<SortModel> sortModel;
        private int startRow, endRow;
        private List<ColumnVO> rowGroupCols;
        private ConcurrentDictionary<string, List<string>> pivotValues;
        private bool isPivotMode;

        public string CreateSQL(Request request, string tableName, ConcurrentDictionary<string, List<string>> pivotValues)
        {
            this.valueColumns = request.ValueCols;
            this.pivotColumns = request.PivotCols;
            this.groupKeys = request.GroupKeys;
            this.rowGroupCols = request.RowGroupCols;
            this.pivotValues = pivotValues;
            this.isPivotMode = request.PivotMode;
            this.rowGroups = getRowGroups();
            this.rowGroupsToInclude = getRowGroupsToInclude();
            this.isGrouping = rowGroups.Count > groupKeys.Count;
            this.filterModel = request.FilterModel;
            this.sortModel = request.SortModel;
            this.startRow = request.StartRow;
            this.endRow = request.EndRow;

            return selectSql() + fromSql(tableName) + whereSql() + groupBySql() + orderBySql() + limitSql();
        }
        public string WhereSQL(Request request, string tableName)
        {
            this.valueColumns = request.ValueCols;
            this.pivotColumns = request.PivotCols;
            this.groupKeys = request.GroupKeys;
            this.rowGroupCols = request.RowGroupCols;
            this.isPivotMode = request.PivotMode;
            this.rowGroups = getRowGroups();
            this.rowGroupsToInclude = getRowGroupsToInclude();
            this.isGrouping = rowGroups.Count > groupKeys.Count;
            this.filterModel = request.FilterModel;
            this.sortModel = request.SortModel;
            this.startRow = request.StartRow;
            this.endRow = request.EndRow;

            return whereSql();
        }

        private string selectSql()
        {
            List<string> selectCols;
            if (isPivotMode && pivotColumns.Count > 0)
            {
                selectCols = (rowGroupsToInclude.Concat(extractPivotStatements())).ToList();
            }
            else
            {
                List<string> valueCols = valueColumns.Select(valueCol => valueCol.AggFunc + '(' + valueCol.Field + ") as " + valueCol.Field).ToList();

                selectCols = (rowGroupsToInclude.Concat(valueCols)).ToList();
            }

            return isGrouping ? "SELECT " + string.Join(", ", selectCols) : "SELECT *";
        }

        private string fromSql(string tableName)
        {
            return string.Format(" FROM {0}", tableName);
        }

        private string whereSql()
        {
            string whereFilters = string.Join(" AND ", (getGroupColumns().Concat(getFilters())).ToList());
            return whereFilters.Length == 0 ? "" : string.Format(" WHERE {0}", whereFilters);
        }

        private string groupBySql()
        {
            return isGrouping ? " GROUP BY " + string.Join(", ", rowGroupsToInclude) : "";
        }

        private string orderBySql()
        {
            Func<SortModel, string> orderByMapper = model => model.ColId + " " + model.Sort;

            bool isDoingGrouping = rowGroups.Count > groupKeys.Count;
            int num = isDoingGrouping ? groupKeys.Count + 1 : Int32.MaxValue;

            List<string> orderByCols = sortModel
                    .Where(model => !isDoingGrouping || rowGroups.Contains(model.ColId))
                    .Select(orderByMapper)
                    .Take(num).ToList();

            return orderByCols.Count == 0 ? "" : " ORDER BY " + string.Join(",", orderByCols);
        }

        private string limitSql(string dbName = "MYSQL")
        {
            return dbName switch
            {
                "ORACLE" => " OFFSET " + startRow + " ROWS FETCH NEXT " + (endRow - startRow + 1) + " ROWS ONLY",
                "SQLSERVER" => " OFFSET " + startRow + " ROWS FETCH NEXT " + (endRow - startRow + 1) + " ROWS ONLY",
                "MYSQL" => " LIMIT " + startRow + " , " + (endRow - startRow + 1),
                _ => " LIMIT " + startRow + " , " + (endRow - startRow + 1)
            };
        }
        public string createFilterSql(string key, dynamic item)
        {
            switch (item.filterType)
            {
                case "text":
                    return this.createTextFilterSql(key, item);
                case "number":
                    return this.createNumberFilterSql(key, item);
                default:
                    Console.WriteLine("unkonwn filter type: " + item.filterType);
                    return "";
            }
        }

        public string createNumberFilterSql(string key, dynamic item)
        {
            switch (item.type)
            {
                case "equals":
                    return key + " = " + item.filter;
                case "notEqual":
                    return key + " != " + item.filter;
                case "greaterThan":
                    return key + " > " + item.filter;
                case "greaterThanOrEqual":
                    return key + " >= " + item.filter;
                case "lessThan":
                    return key + " < " + item.filter;
                case "lessThanOrEqual":
                    return key + " <= " + item.filter;
                case "inRange":
                    return "(" + key + " >= " + item.filter + " and " + key + " <= " + item.filterTo + ")";
                default:
                    Console.WriteLine("unknown number filter type: " + item.type);
                    return "true";
            }
        }

        public string createTextFilterSql(string key, dynamic item)
        {
            switch (item.type)
            {
                case "equals":
                    return key + " = '" + item.filter + "'";
                case "notEqual":
                    return key + " != '" + item.filter + "'";
                case "contains":
                    return key + " like '%" + item.filter + "%'";
                case "notContains":
                    return key + " not like '%" + item.filter + "%'";
                case "startsWith":
                    return key + " like '" + item.filter + "%'";
                case "endsWith":
                    return key + " like '%" + item.filter + "'";
                default:
                    Console.WriteLine("unknown text filter type: " + item.type);
                    return "true";
            }
        }
        private List<string> getFilters()
        {
            Func<KeyValuePair<string, ColumnFilter>, string> applyFilters = entry =>
            {
                string columnName = entry.Key;
                ColumnFilter filter = entry.Value;

                if (filter is SetColumnFilter)
                {
                    return setFilter()(columnName, (SetColumnFilter)filter);
                }

                if (filter is NumberColumnFilter)
                {
                    return numberFilter()(columnName, (NumberColumnFilter)filter);
                }

                return "";
            };

            return filterModel.Select(applyFilters).ToList();
        }

        private Func<string, SetColumnFilter, string> setFilter()
        {
            return (string columnName, SetColumnFilter filter) =>
                    columnName + (filter.Values.Count == 0 ? " IN ('') " : " IN " + asstring(filter.Values));
        }

        private Func<string, NumberColumnFilter, string> numberFilter()
        {
            return (string columnName, NumberColumnFilter filter) =>
            {
                int filterValue = filter.Filter;
                string filerType = filter.FilterType;
                string op = this.operatorMap[filerType];

                return columnName + (filerType.Equals("inRange") ?
                        " BETWEEN " + filterValue + " AND " + filter.FilterTo : " " + op + " " + filterValue);
            };
        }

        private List<string> extractPivotStatements(string func = "CASE")
        {
            List<IEnumerable<KeyValuePair<string, string>>> pivotPairs =
            pivotValues.Select(e => e.Value.ToList().Select(pivotValue => new KeyValuePair<string, string>(e.Key, pivotValue))).ToList();

            return pivotPairs.CartesianProduct().ToList().SelectMany(pairs =>
            {
                string pivotColStr = string.Join("|", pairs.Select(pr => pr.Value));
                string decodeStr = string.Empty;
                string closingBrackets = string.Empty;
                return func switch
                {
                    "IF" => valueColumns.Select(valueCol => valueCol.AggFunc + "(" + string.Join(",", pairs.Select(pair => "IF(" + pair.Key + " = '" + pair.Value + "'")) + ", " + valueCol.Field +
                           string.Join("", Enumerable.Range(0, pairs.Count()).Select(i => ",0)")) + ") \"" + pivotColStr + "|" + valueCol.Field + "\""),
                    "CASE" => valueColumns.Select(valueCol => valueCol.AggFunc + "(" + string.Join(" ", pairs.Select(pair => "(CASE WHEN " + pair.Key + " = '" + pair.Value + "' THEN")) + " " + valueCol.Field +
                           string.Join("", Enumerable.Range(0, pairs.Count()).Select(i => " END )")) + ") \"" + pivotColStr + "|" + valueCol.Field + "\""),
                    "DECODE" => valueColumns.Select(valueCol => valueCol.AggFunc + "(" + string.Join(",", pairs.Select(pair => "DECODE(" + pair.Key + ", '" + pair.Value + "'")) + ", " + valueCol.Field +
                            string.Join("", Enumerable.Range(0, pairs.Count() + 1).Select(i => ")")) + " \"" + pivotColStr + "|" + valueCol.Field + "\""),
                    _ => valueColumns.Select(valueCol => valueCol.AggFunc + "(" + string.Join(" ", pairs.Select(pair => "(CASE WHEN " + pair.Key + " = '" + pair.Value + "' THEN")) + " " + valueCol.Field +
                           string.Join("", Enumerable.Range(0, pairs.Count()).Select(i => " END )")) + ") \"" + pivotColStr + "|" + valueCol.Field + "\"")
                };

            }).ToList();
        }

        private List<string> getRowGroupsToInclude()
        {
            return rowGroups
                    .Take(groupKeys.Count + 1).ToList();
        }

        private List<string> getGroupColumns()
        {
            groupKeys.Zip(rowGroups, (key, group) => group + " = '" + key + "'").ToList().ForEach(p => Console.WriteLine(p));
            return groupKeys.Zip(rowGroups, (key, group) => group + " = '" + key + "'").ToList();
        }

        private List<string> getRowGroups()
        {

            return rowGroupCols.Select(ColumnVO => ColumnVO.Field).ToList();

        }

        private string asstring(List<string> l)
        {
            return "(" + string.Join(", ", l.Select(s => "\'" + s + "\'")) + ")";
        }

        private Dictionary<string, string> operatorMap = new Dictionary<string, string>() {
        {"equals", "="},
        {"notEqual", "<>"},
        {"lessThan", "<"},
        {"lessThanOrEqual", "<="},
        {"greaterThan", ">"},
        {"greaterThanOrEqual", ">="}

        };
    }

}