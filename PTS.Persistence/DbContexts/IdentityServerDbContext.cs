using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PTS.Persistence.Models.Users;

namespace PTS.Persistence.DbContexts;
public class IdentityServerDbContext : IdentityDbContext<ApplicationUser>
{
    public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options) : base(options)
    {
    }
}
