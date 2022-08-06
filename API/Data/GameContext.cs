
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class GameContext: DbContext
    {
        public GameContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Player> player { get; set; }
    }
}