namespace ChatroomApi.Data
{
    using System.IO;
    using ChatroomApi.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class ChatContext : DbContext
    {
        public ChatContext() : base()
        {
            this.Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("ChatConnectionString");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
