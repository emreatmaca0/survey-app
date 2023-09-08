using Microsoft.EntityFrameworkCore;

namespace anket_kazan.Models
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }


        public DbSet<Users> Users { get; set; }
        public DbSet<Survey> Survey { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }
    }
}
