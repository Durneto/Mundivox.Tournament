using Microsoft.EntityFrameworkCore;
using Mundivox.Tournament.Model;

namespace Mundivox.Tournament.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Team { get; set; }
        public DbSet<Phase> Phase { get; set; }
        public DbSet<Key> Key { get; set; }
    }
}
