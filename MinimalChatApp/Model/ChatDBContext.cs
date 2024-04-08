using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinimalChatApp.Model;

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
        public DbSet<Logs> Logs { get; set; }

        public DbSet <ErrorLogger > ErrorLogs { get; set; }
    }
}
