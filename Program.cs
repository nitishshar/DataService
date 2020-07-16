using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataService.Helpers;
using DataService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var request = new Request();
            request.setStartRow(0);
            request.setEndRow(100);
            request.setRowGroupCols(new List<ColumnVO>(){
                new ColumnVO("COUNTRY", "Country", "COUNTRY", "")
        }
            );
            request.setValueCols(new List<ColumnVO>(){
                    new ColumnVO("GOLD", "Gold", "GOLD", "sum"),
                    new ColumnVO("SILVER", "Silver", "SILVER", "sum"),
                    new ColumnVO("BRONZE", "Bronze", "BRONZE", "sum"),
                    new ColumnVO("TOTAL", "Total", "TOTAL", "sum")
            });
            request.setFilterModel(new Dictionary<string, ColumnFilter>() {
            {"SPORT", new SetColumnFilter(new List<string>(){"Rowing", "Tennis"})},
            {"AGE", new NumberColumnFilter("equals", 22, 26)}
        });
            request.setSortModel(new List<SortModel>() { new SortModel("ATHLETE", "asc") });
            request.setPivotMode(true);
            request.setPivotCols(new List<ColumnVO>(){
                new ColumnVO("SPORT", "Sport", "SPORT", null)
            });
            Dictionary<String, List<String>> pivotValues = new Dictionary<string, List<string>>();
            pivotValues.Add("SPORT", new List<string>() { "Athletics", "Speed Skating" });
            pivotValues.Add("YEAR", new List<string>() {"2000", "2004"});
            String sql = new SQLBuilder().CreateSQL(request, "medal", pivotValues);
            Console.WriteLine(sql);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
