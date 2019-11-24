using System.Linq;
using DataLoggerApp.Models;
using DataLoggerApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataLoggerApp.Controllers
{
    [Route("api/[controller]")]
   
    [Route("[controller]")]
    [ApiController]
    public class LoggerTraceController : ControllerBase
    {
       

        private readonly ILoggerService _loggerService;
        private readonly ITraceService  _traceService;
        
        
        
            [HttpGet]
        [Route("LoggerTrace")]
        public ActionResult<string> Get()
        {
            return "Running....";
        }
       
        public LoggerTraceController(ILoggerService loggerService, ITraceService traceService)
        {
            this._loggerService = loggerService;
            this._traceService = traceService;
        }

        [HttpPost]
        [Route("DataLogger")]
        [Route("api/[controller]/DataLogger")]
        public ActionResult<object> DataLogger([FromBody]DatasList request)
        {
            request.TotalCount = request.Data.Count();

            _loggerService.CreateFilesByDataType(request);
            

            return Ok(request);
        }

        [HttpPost]
        [Route("DataLoggerTrace")]
        [Route("api/[controller]/DataLoggerTrace")]
        public ActionResult<object> DataLoggerTrace()
        {
            _traceService.CreateOrUpdateLoggerTraceInDirectory();
            
            return Ok("Completed Successfully");
        }

      

       
    }

   
}