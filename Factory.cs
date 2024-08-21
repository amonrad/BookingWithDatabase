using Microsoft.EntityFrameworkCore;

namespace BookingWithDatabase
{
	public static class Factory
	{

        // gathers all dependencies in one place in the application:

        public static Meeting CreateMeeting(Bookable client, DateTime dateTime, int duration, Bookable employee, Bookable location)
		{
			return new Meeting(client, dateTime, duration, employee, location);
		}

		public static Location CreateLocation(Guid? ID, string name)
		{
			return new Location(ID, name);
		}

		public static Employee CreateEmployee(Guid? ID, string name)
		{
			return new Employee(ID, name);
		}

        public static Client CreateClient(Guid? ID, string name)
        {
            return new Client(ID, name);
        }

		public static IDataCollectionForm CreateDateCollectionForm()
		{
			return new DataCollectionForm();
		}

		public static IDisplay CreateDisplay(IAppDbContext DbContext)
		{
			return new Display(DbContext);
		}

		public static IHandleBooking CreateHandleBooking(IAppDbContext DbContext)
		{
			return new HandleBooking(DbContext);
		}

        public static INewBookingsForm CreateNewBookingsForm(IAppDbContext DbContext, List<Bookable> locations, List<Bookable> employees, List<Bookable> clients, IHandleBooking handleBooking)
        {
            return new NewBookingsForm(DbContext, locations, employees, clients, handleBooking);
        }

        public static IReception CreateReception(IAppDbContext DbContext)
        {
            return new Reception(DbContext);
        }

		public static IDictionary<string, List<Meeting>> createDictionaryOfMeetings()
		{
			return new Dictionary<string, List<Meeting>>();
        }

		public static List<Bookable> createListOfBookables()
		{
			return new List<Bookable>();
        }

		public static List<Meeting> createListOfMeeting()
		{
			return new List<Meeting>();
		}

		public static TimeSpan createNewTimeSpan(int hours, int minutes, int seconds)
		{
			return new TimeSpan(hours, minutes, seconds);
        }

		public static Random createNewRandom()
		{
			return new Random();
		}

		public static DateTime createNewDateTime(int year, int month, int day, int hour, int minute, int second)
		{
			return new DateTime(year, month, day, hour, minute, second);
        }

		public static IAppDbContext CreateNewAppDbContext(string connectionString)
    	{
        	var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        	optionsBuilder.UseNpgsql(connectionString);

        	return new AppDbContext(optionsBuilder.Options);
    	}

		public static CleanUpPastMeetings cleanUp(IAppDbContext DbContext){
			return new CleanUpPastMeetings(DbContext);
		}
    }
}

