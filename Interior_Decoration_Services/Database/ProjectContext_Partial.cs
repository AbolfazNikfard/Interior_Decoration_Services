using Interior_Decoration_Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Interior_Decoration_Services.Data
{
    public partial class ProjectContext
    {
        internal void AddModelCreatingConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsDelete == false); //&& p.confirmation == AcceptProduct.Accept);
            // modelBuilder.Entity<Seller>().HasQueryFilter(s => s.IsDelete == false);
            modelBuilder.Entity<Buyer>().HasQueryFilter(b => b.IsDelete == false);
            modelBuilder.Entity<Group>().HasQueryFilter(g => g.IsDelete == false);
            modelBuilder.Entity<SubGroup>().HasQueryFilter(sg => sg.IsDelete == false);            
        }
    }
}
