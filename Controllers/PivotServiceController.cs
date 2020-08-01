using System.Threading.Tasks;
using DataService.Domain;
using DataService.Helpers;
using DataService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DataService.Controllers
{
    [Route("api/[controller]")]
    public class PivotServiceController : ControllerBase
    {
        public DBConnector Db { get; }
        private readonly ILogger _logger;

        private IMemoryCache _memoryCache;

        public PivotServiceController(DBConnector db, IMemoryCache memorycache, ILogger<PivotServiceController> logger)
        {
            this.Db = db;
            this._memoryCache = memorycache;
            this._logger = logger;
        }


        // POST api/getRows
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Request request)
        {
            _logger.LogInformation("Request:" + JsonSerializer.Serialize(request));
            await Db.Connection.OpenAsync();
            var pivotApi = new PivotServiceApi(this.Db, this._memoryCache);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true

            };
            var response = await pivotApi.GetRows(request);
            return new OkObjectResult(response);
        }

        [HttpGet]
        public IActionResult IsServiceUp()
        {
            return new OkObjectResult("The DataService is up and running.");
        }


    }
}