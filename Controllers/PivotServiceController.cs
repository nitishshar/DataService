using System.Threading.Tasks;
using DataService.Domain;
using DataService.Helpers;
using DataService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace DataService.Controllers
{
    [Route("api/[controller]")]
    public class PivotServiceController : ControllerBase
    {
        public DBConnector Db { get; }
        public PivotServiceController(DBConnector db)
        {
            this.Db = db;
        }


        // POST api/getRows
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Request request)
        {

            await Db.Connection.OpenAsync();
            var pivotApi = new PivotServiceApi(this.Db);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
                
            };           
            var response = await pivotApi.GetRows(request);
            return new OkObjectResult(response);
        }

        [HttpGet]
        public IActionResult GetLatest()
        {
            return new OkObjectResult("Hello Api");
        }


    }
}