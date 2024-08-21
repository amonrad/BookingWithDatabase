namespace BookingWithDatabase
{
    public interface IDataCollectionForm
    {
        Bookable GetClientInput(List<Bookable> existingClients, IAppDbContext DbContext);
        DateTime getDateInput();
        int getDurationInput();
        Bookable getEmployeeInput(List<Bookable> employees);
        Bookable getLocationInput(List<Bookable> locations);
        DateTime GetRandomDateTime();
        int GetRandomDuration();
        T GetRandomElement<T>(List<T> list);
    }
}