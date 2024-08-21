using Microsoft.EntityFrameworkCore;

namespace BookingWithDatabase
{
    public interface IAppDbContext
    {
        //public DbSet<Person> Person { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        // Define other methods or properties as needed
    }
}