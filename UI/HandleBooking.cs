namespace BookingWithDatabase
{
    public class HandleBooking : IHandleBooking
    {
        // Fields:
        List<Meeting> meetings;
        private string rejectionCause = "";
        private readonly IAppDbContext _dbContext;

        // Constructor:

        public HandleBooking(IAppDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        // Public methods:

        // REQUIRES: A valid meeting.
        // EFFECTS: Indicates whether the booking is possible or not.

        public bool isRequestedBookingPossible(Meeting requestedMeeting)
        {
            if (requestedMeeting == null || requestedMeeting.Location == null || requestedMeeting.Employee == null || requestedMeeting.Client == null)
            {
                // Handle if requestedMeeting is null
                throw new ArgumentNullException("requestedMeeting", "The meeting or its properties cannot be null.");
            }

            bool isBookingPossible = true;

            // Create a list of bookables
            List<Bookable> bookables = Factory.createListOfBookables();
            bookables.Add(requestedMeeting.Location);
            bookables.Add(requestedMeeting.Employee);
            bookables.Add(requestedMeeting.Client);

            foreach (Bookable bookable in bookables)
            {
                // Check if bookables do NOT have availability in their calendar for the requested meeting
                if (!bookable.isBookingPossible(requestedMeeting, _dbContext))
                {
                    isBookingPossible = false;
                    // Add an explanation to the rejection of the meeting
                    requestedMeeting.addRejectionCause(bookable.explanation());
                }
            }
            // Add validation to the meeting object
            requestedMeeting.isPossible = isBookingPossible;

            return isBookingPossible;
        }

        // REQUIRES: A valid meeting.
        // MODIFIES: Modifies the calendar for each Bookable instance and the meeting calendar.
        // EFFECTS: Adds the meeting to the calendar, throws exception if the meeting is not validated.

        public void AddMeetingToTable(Meeting requestedMeeting)
        {
            if (requestedMeeting == null || requestedMeeting.Location == null || requestedMeeting.Employee == null || requestedMeeting.Client == null)
            {
                // Handle if requestedMeeting is null
                throw new ArgumentNullException("requestedMeeting", "The meeting or its properties cannot be null.");
            }

             // Check that the meeting is validated before adding it to the database
            if (requestedMeeting.isPossible)
            {
                 // Add the meeting to the database
                _dbContext.Meetings.Add(requestedMeeting);
                _dbContext.SaveChanges();
            }
            else
            {
                // Handle the case where the meeting is not possible (validation failed)
                throw new InvalidOperationException("The meeting cannot be added to the calendar because it is not validated.");
            }
        }
    }
}