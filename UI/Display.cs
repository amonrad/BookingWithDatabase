using Microsoft.EntityFrameworkCore;

namespace BookingWithDatabase
{
    public class Display : IDisplay
    {
        private readonly IAppDbContext _dbContext;

        // Constructor:

        public Display(IAppDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        // Public Method:

        // EFFECTS: Prints the calendar to the console in chronological order
        public void DisplayMeetingCalendar()
        {
            try
            {
                // Fetch all meetings from the database
                var meetings = _dbContext.Meetings
                                        .Include(m => m.Client)
                                        .Include(m => m.Employee)
                                        .Include(m => m.Location)
                                        .OrderBy(m => m.Start)
                                        .ToList();

                // Check if there are any meetings
                if (meetings.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("The meeting calendar is empty, no meetings are booked.");
                    Console.WriteLine("\nPress any key to return to the menu.");
                    Console.ReadKey();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Meeting Overview: \n");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine($" {"Date",-20} {"Time",-20} {"Client",-20} {"Employee",-20} {"Location",-20}");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------\n");

                    // Print each meeting
                    foreach (var meeting in meetings)
                    {
                        Console.WriteLine($" {meeting.Start.ToString("dd/MM-yyyy"),-20} {meeting.Start.ToString("HH:mm")}-{meeting.End.ToString("HH:mm"),-20} {meeting.Client.name,-20} {meeting.Employee.name,-20} {meeting.Location.name,-20}\n");
                    }

                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");

                    Console.WriteLine("\nPress any key to return to the menu.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}