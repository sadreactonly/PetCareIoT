using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetCareIoTMiddleware.Models;

namespace PetCareIoTMiddleware.Authentication
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BaseEvent> Events { get; set; }

        public void MarkAsModified(object item)
        {
            Entry(item).State = EntityState.Modified;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}