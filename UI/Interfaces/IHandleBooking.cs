namespace BookingWithDatabase
{
    public interface IHandleBooking
    {
        void AddMeetingToTable(Meeting requestedMeeting);
        bool isRequestedBookingPossible(Meeting requestedMeeting);
    }
}