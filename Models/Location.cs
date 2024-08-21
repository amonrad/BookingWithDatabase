namespace BookingWithDatabase
{
    public class Location : Bookable
    {
        // Constructor:

        public Location(Guid? id, string name) : base(id, name)
        {
        }


        // Public Methods:

        // EFFECTS: Override method that returns a specific explanation for location objects.

        public override string explanation()
        {
            string explanation = "\n- the room " + name + " is not available at this time.";
            return explanation;
        }
    }
}