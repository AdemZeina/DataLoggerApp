using System.Linq;
using DataLoggerApp.Models;
using DataLoggerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataLoggerApp.Controllers
{
    [Route("api/[controller]")]
    [Route("LoggerTrace")]
    [Route("[controller]")]
    [ApiController]
    public class LoggerTraceController : ControllerBase
    {
       

        private readonly ILoggerService loggerService;
        private readonly ITraceService  traceService;
        [HttpGet]
        [Route("LoggerTrace")]
        [Route("[controller]")]
        public ActionResult<string> Get()
        {
            return "Running....";
        }
       
        public LoggerTraceController(ILoggerService loggerService, ITraceService traceService)
        {
            this.loggerService = loggerService;
            this.traceService = traceService;
        }

        [HttpPost]
        [Route("DataLogger")]
        [Route("api/[controller]/DataLogger")]
        public ActionResult<object> DataLogger([FromBody]DatasList request)
        {
            request.TotalCount = request.Data.Count();

            loggerService.CreateFilesByDataType(request);
            

            return Ok(request);
        }

        [HttpPost]
        [Route("DataLoggerTrace")]
        [Route("api/[controller]/DataLoggerTrace")]
        public ActionResult<object> DataLoggerTrace()
        {
            traceService.CreateOrUpdateLoggerTraceInDirectory();
            return Ok("Completed Successfully");
        }

      

       
    }

   
}