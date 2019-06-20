using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BackgroundTaskWithHostedService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            ContentResult cr = new ContentResult();
            cr.Content = "Welcome to Bgwhs API \r\nVersion:" + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            return cr;
        }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly ILogger<SyncController> _logger;
        public SyncController(ILogger<SyncController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("SynchronizeUserData")]
        public async Task<string> SynchronizeUserData(string UserId)
        {
            var UserIdTimestamp = string.Format("Sync -> {0}_{1} {2}", UserId,DateTime.Now.Ticks, System.Environment.NewLine);
            using (StreamWriter writer = System.IO.File.AppendText("db.txt"))
            {
                writer.WriteLine(UserIdTimestamp);
            }
            return UserIdTimestamp;
        }
    }
}