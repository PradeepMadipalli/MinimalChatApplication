using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalChatApplication.Model;

namespace MinimalChatApp.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class LogsController : ControllerBase
    {
        private readonly ChatDBContext _dBContext;

        public LogsController(ChatDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        [HttpGet]
        [Route("Logs")]
        public async Task<IActionResult> GetLogs(DateTime? startTime = null, DateTime? endTime = null)
        {
            if (startTime == null)
                startTime = DateTime.Now.AddMinutes(-5);

            if (endTime == null)
                endTime = DateTime.Now;

            var logs = await _dBContext.Logs
                .Where(l => l.CreatedDate >= startTime && l.CreatedDate <= endTime)
                .ToListAsync();

            if (logs.Count == 0)
                return NotFound(new { error = "No logs found" });

            return Ok(new { Logs = logs });
        }
    }
}
