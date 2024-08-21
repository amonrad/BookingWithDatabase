namespace BookingWithDatabase
{
    public class CleanUpPastMeetings
    {
        private readonly IAppDbContext _dbContext;

        public CleanUpPastMeetings(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CleanUp()
        {
            // Get the current date and time
            DateTime currentDate = DateTime.UtcNow;

            // Query the Meeting table for past meetings
            var pastMeetings = _dbContext.Meetings
                .Where(m => m.Start < currentDate)
                .ToList();

            // Remove past meetings from the context
            _dbContext.Meetings.RemoveRange(pastMeetings);

            // Save the changes to the database
            _dbContext.SaveChanges();
        }
    }
}