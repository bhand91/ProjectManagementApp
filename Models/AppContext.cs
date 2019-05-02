using Microsoft.EntityFrameworkCore;

namespace ProjectManagementApp.Models
{
    public class WebAppContext : DbContext
    {
        public WebAppContext(DbContextOptions<WebAppContext> options) : base(options)
        {

        }

        public DbSet<Project> Projects {get; set;}
        public DbSet<Member> Members {get; set;}

        public DbSet<ProjectMember> ProjectMembers {get; set;}

    }
}