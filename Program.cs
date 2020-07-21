using System;
using System.Collections.Generic;
using System.Text.Json;
using DataService.Helpers;
using DataService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace DataService
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var request = new Request();
            request.StartRow = 0;
            request.EndRow = 100;
            request.RowGroupCols = new List<ColumnVO>(){
                new ColumnVO("1", "Employee Number", "emp_no", ""),
                new ColumnVO("2", "First Name", "first_name", ""),
                 new ColumnVO("2", "last name", "last_name", "")
            };

            request.ValueCols = new List<ColumnVO>(){
                    new ColumnVO("3", "Salary", "salary", "sum")
            };
            request.FilterModel = new Dictionary<string, ColumnFilter>() {
            {"title", new SetColumnFilter(new List<string>(){"Senior Engineer","Staff"},"set")}
            };
            //request.setSortModel(new List<SortModel>() { new SortModel("first_name", "asc") });
            request.PivotMode = true;
            request.PivotCols = new List<ColumnVO>(){
                new ColumnVO("4", "Title", "title", null)
            };
            //request.setGroupKeys(new List<string>(){"91966","220771"});
            Dictionary<String, List<String>> pivotValues = new Dictionary<string, List<string>>();
            pivotValues.Add("title", new List<string>() { "Senior Engineer", "Staff" });
            pivotValues.Add("last_name", new List<string>() { "Facello", "Simmel" });

            Console.WriteLine(JsonSerializer.Serialize<Request>(request));
            String sql = new SQLBuilder().CreateSQL(request, "employeedetails", pivotValues);
            Console.WriteLine(sql);
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
