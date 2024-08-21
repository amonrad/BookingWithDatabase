// Program.cs

using Microsoft.EntityFrameworkCore;
using BookingWithDatabase;

namespace BookingWithDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            // Field:

            string connectionString = "Host=localhost; Database=Booking; Username=postgres; Password=EdgarWinter1";

            // Create instance of AppDbContext with configured options
            using (var dbContext = Factory.CreateNewAppDbContext(connectionString) as AppDbContext)
            {
                // Your code using dbContext here
                //dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                IReception reception = Factory.CreateReception(dbContext);

                // Create object to remove passed meetings from database
                CleanUpPastMeetings cleanUp = Factory.cleanUp(dbContext);
                // remove passed meetings from database
                cleanUp.CleanUp();

                // Print welcome message in console
                reception.Welcome();

                // Program flow is controlled by reception.Menu()
                reception.Menu();
            }
        }
    }
}