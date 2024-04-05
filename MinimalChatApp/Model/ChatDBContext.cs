using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MinimalChatApplication.Model
{
    public class ChatDBContext:IdentityDbContext<IdentityUser>
    {
        public ChatDBContext(DbContextOptions<ChatDBContext> options):base(options) { 
        
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Message> Messages { get; set; }
    }
}
