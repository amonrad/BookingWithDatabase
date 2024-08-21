using Microsoft.EntityFrameworkCore;

namespace BookingWithDatabase
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        
        // constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        /*public void SaveChanges()
        {
            base.SaveChanges();
        }*/


         // DbSet properties representing your entities

        //public DbSet<Person> Person { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Meeting> Meetings { get; set; }


        // Override the OnConfiguring method
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure the PostgreSQL provider and connection string
                optionsBuilder.UseNpgsql("Host=localhost; Database=Booking; Username=postgres; Password=u19f8fj4i98d8hbjs8");
            }
        }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
            base.OnModelCreating(modelBuilder);

            // Configure the inheritance hierarchy using TPH strategy
            modelBuilder.Entity<Bookable>()
                .HasDiscriminator<string>("BookableType")
                .HasValue<Client>("Client")
                .HasValue<Location>("Location")
                .HasValue<Employee>("Employee");

            // Configure the Meeting entity
            modelBuilder.Entity<Meeting>()
                .ToTable("Meetings") // Specify the table name
                .HasKey(m => m.Id); // Specify the primary key

            // Configure relationships between entities
            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Client)
                .WithMany()
                .HasForeignKey(m => m.ClientId);

            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Employee)
                .WithMany()
                .HasForeignKey(m => m.EmployeeId);

            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Location)
                .WithMany()
                .HasForeignKey(m => m.LocationId);

            // Add additional configurations as needed
        }
    }
}