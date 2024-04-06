using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinimalChatApp.Model;

namespace MinimalChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private List<Log> logs = new List<Log>();

        [HttpGet]
        [Authorize]
        public IActionResult GetLogs(DateTime? startTime = null, DateTime? endTime = null)
        {

            if (endTime != null && startTime != null && endTime < startTime)
            {
                return BadRequest("EndTime must be greater than or equal to StartTime.");
            }


            var filteredLogs = logs.FindAll(log =>
                (!startTime.HasValue || log.RequestTime >= startTime) &&
                (!endTime.HasValue || log.RequestTime <= endTime)
            );

            if (filteredLogs.Count == 0)
            {
                return NotFound("No logs found.");
            }

            return Ok(filteredLogs);

        }
    }
}
