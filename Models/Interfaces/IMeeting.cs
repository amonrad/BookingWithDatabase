namespace BookingWithDatabase
{
    public interface IMeeting
    {
        Guid Id { get; }
        Bookable Client { get; set; }
        Bookable Employee { get; set; }
        Bookable Location { get; set; }
        DateTime Start { get; set; }
        int Duration { get; set; }
        DateTime End { get; }
        string meetingConfirmation();
        string meetingRejection();
        void addRejectionCause(string cause);
        string timeSlot { get; }
        string date { get; }
        bool isPossible { get; set; }
    }
}