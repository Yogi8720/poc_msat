using Microsoft.EntityFrameworkCore;

namespace poc_msat.Models
{
    public class CosmosDBContext:DbContext
    {
        public CosmosDBContext(DbContextOptions<CosmosDBContext> option) : base(option)
        {
        }

        public DbSet<Order> Orders { set; get; }
        public DbSet<OrderItem> OrderItems { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(o=> {
                o.ToContainer("Order");
                o.HasPartitionKey(x=> x.PartitionKey);
                o.OwnsMany(x => x.OrderItems);
                //o.OwnsMany(x => x.SelectedOrderItems);
                o.OwnsOne(x => x.Customer, n => 
                n.OwnsOne(a => a.Address)) ;
            });
        }

        public DbSet<Customer>Customers { get; set; }
    }
}
