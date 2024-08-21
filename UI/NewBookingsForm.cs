namespace BookingWithDatabase
{
    public class NewBookingsForm : INewBookingsForm
    {
        // Fields:

        List<Bookable> locations;
        List<Bookable> employees;
        List<Bookable> clients;

        private IHandleBooking handleBooking;
        private IDataCollectionForm dataCollectionForm;
        private readonly IAppDbContext _dbContext;

        // Constructor:

        public NewBookingsForm(IAppDbContext DbContext, List<Bookable> locations, List<Bookable> employees, List<Bookable> clients, IHandleBooking handleBooking)
        {
            _dbContext = DbContext;

            this.locations = locations;
            this.employees = employees;
            this.clients = clients;
            this.handleBooking = handleBooking;

            // Create an object for data collection
            dataCollectionForm = Factory.CreateDateCollectionForm();
        }

        // Public methods:

        // REQUIRES: User input for selecting client, date, duration, employee, and location.
        // MODIFIES: Potentially adds a meeting to the calendar.
        // EFFECTS: Creates a meeting, validates it, and then adds it to the calendar or rejects it. Also prints a confirmation or rejection message to the console.

        public void fillBookingForm()
        {
            // Obtain data about client, date, duration, employee, and location via user input
            Bookable selectedClient = dataCollectionForm.GetClientInput(clients, _dbContext);
            DateTime selectedDateTime = dataCollectionForm.getDateInput();

            int selectedDuration = dataCollectionForm.getDurationInput();
            Bookable selectedEmployee = dataCollectionForm.getEmployeeInput(employees);
            Bookable selectedLocation = dataCollectionForm.getLocationInput(locations);

            // Create a meeting for validation
            Meeting requestedMeeting = Factory.CreateMeeting(selectedClient, selectedDateTime, selectedDuration, selectedEmployee, selectedLocation);

            // Add the meeting to the calendar if validated
            if (handleBooking.isRequestedBookingPossible(requestedMeeting))
            {
                handleBooking.AddMeetingToTable(requestedMeeting);
                // Print confirmation to console
                printResponse(requestedMeeting.meetingConfirmation());
            }
            else
            {
                // Print rejection to console
                printResponse(requestedMeeting.meetingRejection());
            }
        }

        // REQUIRES: User input for selecting client.
        // MODIFIES: Adds a meeting to the calendar.
        // EFFECTS: Generates and validates a random booking, adds the meeting to the calendar, and prints a confirmation message to the console.

        public void GenerateRandomBooking()
        {
            // Add client to the new meeting
            Bookable selectedClient = dataCollectionForm.GetClientInput(clients, _dbContext);

            // Generate fields and validate random booking

            Meeting requestedMeeting = null;

            while (requestedMeeting == null)
            {
                DateTime randomDateTime = dataCollectionForm.GetRandomDateTime();
                int randomDuration = dataCollectionForm.GetRandomDuration();
                Bookable randomEmployee = dataCollectionForm.GetRandomElement(employees);
                Bookable randomLocation = dataCollectionForm.GetRandomElement(locations);

                // Create a meeting for validation
                requestedMeeting = Factory.CreateMeeting(selectedClient, randomDateTime, randomDuration, randomEmployee, randomLocation);

                if (!handleBooking.isRequestedBookingPossible(requestedMeeting))
                {
                    // If the meeting is not possible, set requestedMeeting to null and try again
                    requestedMeeting = null;
                }
            }

            // Now that 'requestedMeeting' is validated, it can be added to the calendar
            handleBooking.AddMeetingToTable(requestedMeeting);

            // Print confirmation to console
            printResponse(requestedMeeting.meetingConfirmation());
        }


        // Private methods:

        // EFFECTS: Clears console, prints string, and awaits user input.

        private static void printResponse(string response)
        {
            Console.Clear();
            Console.WriteLine(response);
            Console.ReadKey();
        }
    }
}