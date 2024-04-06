using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinimalChatApplication.Model;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MinimalChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly ChatDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MessageController(ChatDBContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        [Route("Send")]
        public async Task<IActionResult> SendMessage(MessageRequest request)
        {
            if (ModelState.IsValid)
            {
                
                string user = _httpContextAccessor.HttpContext.User.Identity.Name;
                

                var receiverExists = await _context.Users.AnyAsync(u => u.Id == request.ReceiverId);
                if (!receiverExists)
                {
                    return BadRequest(new { error = "Receiver does not exist." });
                }

                var message = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    SenderId = user,
                    ReceiverId = request.ReceiverId,
                    Content = request.Content,
                    Timestamp = DateTime.UtcNow
                };

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    messageId = message.MessageId,
                    senderId = message.SenderId,
                    receiverId = message.ReceiverId,
                    content = message.Content,
                    timestamp = message.Timestamp
                });
            }
            else
            {
                return BadRequest("Message sending failed due to validation errors");
            }
        }
        
        [HttpPut("Edit/{messageId}")]
        public async Task<IActionResult> EditMessage(string messageId, EditMessageRequest request)
        {

            var senderId = _httpContextAccessor.HttpContext.User.Identity.Name;


            var message = await _context.Messages.FindAsync(messageId);


            if (message == null)
            {
                return NotFound(new { error = "Message not found." });
            }


            if (message.SenderId != senderId)
            {
                return Unauthorized(new { error = "You are not authorized to edit this message." });
            }
            message.Content = request.Content;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    messageId = message.MessageId,
                    senderId = message.SenderId,
                    receiverId = message.ReceiverId,
                    content = message.Content,
                    timestamp = message.Timestamp
                });
            }
            catch (Exception)
            {
                return BadRequest(new { error = "Failed to edit the message." });
            }
        }

        [HttpDelete("Delete/{messageId}")]
        public async Task<IActionResult> DeleteMessage(string messageId)
        {

            var senderId = _httpContextAccessor.HttpContext.User.Identity.Name;


            var message = await _context.Messages.FindAsync(messageId);


            if (message == null)
            {
                return NotFound(new { error = "Message not found." });
            }


            if (message.SenderId != senderId)
            {
                return Unauthorized(new { error = "You are not authorized to delete this message." });
            }


            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Message deleted successfully." });
        }
        [HttpGet]
        [Route("GetConversationHistory")]
        public async Task<IActionResult> GetConversationHistory([FromQuery] ConversationHistoryRequest request)
        {

            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;


            if (request.Count <= 0 || request.Count > 100)
            {
                return BadRequest(new { error = "Invalid count parameter. Count should be between 1 and 100." });
            }

            DateTime beforeDate = request.Before ?? DateTime.UtcNow;
            var sortOrder = request.Sort == "desc" ? SortOrder.Descending : SortOrder.Ascending;




            var messagesQuery = _context.Messages
                   .Where(m => (m.SenderId == userId && m.ReceiverId == request.UserId) || (m.SenderId == request.UserId && m.ReceiverId == userId))
                   .Where(m => m.Timestamp < beforeDate)
                   .OrderBy(m => sortOrder == SortOrder.Ascending ? m.Timestamp : (DateTime?)null)
                   .OrderByDescending(m => sortOrder == SortOrder.Descending ? m.Timestamp : (DateTime?)null)
                   .Take(request.Count);


            var messages = await messagesQuery.Select(m => new
            {
                id = m.MessageId,
                senderId = m.SenderId,
                receiverId = m.ReceiverId,
                content = m.Content,
                timestamp = m.Timestamp
            }).ToListAsync();

            if (messages.Count == 0)
            {
                return NotFound(new { error = "Conversation not found." });
            }

            return Ok(new { messages });
        }
    }
}
