namespace BookingWithDatabase
{
    public interface IBookable
    {
        Guid? id { get; set; }
        string name { get; set; }
        bool isBookingPossible(IMeeting requestedMeeting, IAppDbContext dbContext);
        
        bool isDateTimesOverlapping(DateTime start1, DateTime end1, DateTime start2, DateTime end2);
        string explanation();
    }
}