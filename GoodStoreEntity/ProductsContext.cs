using Microsoft.EntityFrameworkCore;

namespace GoodStoreEntity
{
    public class ProductsContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ProductsContext()
        {
        }

        public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Consignment> Consignment { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Saveliy Kolesnikov\\source\\repos\\GoodStore\\GoodStore\\AppData\\GoodStoreDb.mdf;Integrated Security=True;Connect Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consignment>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Consignments)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Consignme__Produ__33D4B598");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
