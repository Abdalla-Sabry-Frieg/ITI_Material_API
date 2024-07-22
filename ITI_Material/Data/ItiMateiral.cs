using ITI_Material.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITI_Material.Data
{
    public class ItiMateiral: IdentityDbContext<ApplicationUser> 
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public ItiMateiral(DbContextOptions<ItiMateiral> options) :base(options)
        {
            
        }
    }
}
