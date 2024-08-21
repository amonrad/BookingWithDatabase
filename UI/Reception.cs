namespace BookingWithDatabase
{
    public class Reception : IReception
    {
        // Fields:

        List<Bookable> clients;
        List<Bookable> locations;
        List<Bookable> employees;

        IHandleBooking handleBooking;
        INewBookingsForm newBookingsForm;
        IDisplay display;

        private readonly IAppDbContext _dbContext;

        // Constructor:

        public Reception(IAppDbContext DbContext)
        {
            _dbContext = DbContext;

            // Create a new list of clients
            clients = Factory.createListOfBookables();

            //AddClientsToDbContext();

            // create empty lists of locations & employees
            locations = Factory.createListOfBookables();
            employees = Factory.createListOfBookables();

            // Check if there are any existing locations in the database
            var existingLocations = _dbContext.Locations.ToList();
            if (existingLocations.Any())
            {
                // Add existing locations to the list
                locations.AddRange(existingLocations);
            }
            else
            {
                // If no locations exist in the database, create new instances
                foreach (var locationName in new List<string> { "Havesalen", "Riddersalen", "Kælderloungen" })
                {
                    var newLocation = Factory.CreateLocation(Guid.NewGuid(), locationName);
                    locations.Add(newLocation);
                }
                // Add the new locations to the database
                AddBookablesToDbContext(locations);
            }

            // Check if there are any existing employees in the database
            var existingEmployees = _dbContext.Employees.ToList();
            if (existingEmployees.Any())
            {
                // Add existing employees to the list
                employees.AddRange(existingEmployees);
            }
            else
            {
                // If no employees exist in the database, create new instances
                foreach (var employeeName in new List<string> { "Jacob", "Inge", "Morten" })
                {
                    var newEmployee = Factory.CreateEmployee(Guid.NewGuid(), employeeName);
                    employees.Add(newEmployee);
                }
                // Add the new employees to the database
                AddBookablesToDbContext(employees);
            }

            // Create a display to show the meeting list
            display = Factory.CreateDisplay(DbContext);

            // Create an object for meeting booking
            handleBooking = Factory.CreateHandleBooking(DbContext);

            // Create an object for data collection
            newBookingsForm = Factory.CreateNewBookingsForm(DbContext, locations, employees, clients, handleBooking);
        }

        // Public methods:

        // EFFECTS: Prints a welcome message to the console

        public void Welcome()
        {
            Console.Clear();
            Console.WriteLine("\n\rWelcome to booking");
            Console.WriteLine("\n\rPress any key to continue.");
            Console.ReadKey();
        }

        // REQUIRES: User input from a set of options to continue.
        //           Specifically, expects input to correspond to a menu item.
        // EFFECTS: Displays a menu and performs actions based on user input

        public void Menu()
        {
            bool validKeyPressed = false;

            while (!validKeyPressed)
            {
                // Print menu
                Console.Clear();
                Console.WriteLine("\n\rTo book a meeting where you enter booking details yourself, press '1',");
                Console.WriteLine("\n\rTo have us make a booking for you, press '2',");
                Console.WriteLine("\n\rTo see an overview of all bookings, press '3'");
                Console.WriteLine("\n\rTo exit the program, press '4'");

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // Check input and perform action based on input
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        newBookingsForm.fillBookingForm();
                        break;

                    case ConsoleKey.D2:
                        newBookingsForm.GenerateRandomBooking();
                        break;

                    case ConsoleKey.D3:
                        display.DisplayMeetingCalendar();
                        break;

                    case ConsoleKey.D4:
                        validKeyPressed = true;
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("\nInvalid input, press any key to try again");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AddBookablesToDbContext(List<Bookable> bookables)
        {
            foreach (var bookable in bookables)
            {
                if (bookable is Employee employee)
                {
                    _dbContext.Employees.Add(employee);
                }
                else if (bookable is Location location)
                {
                    _dbContext.Locations.Add(location);
                }
                // Add other bookable types as needed
            }
        }
    }
}