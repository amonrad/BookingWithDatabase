namespace BookingWithDatabase
{
    public class Employee : Bookable
    {
        // Constructor:

        public Employee(Guid? id, string name) : base(id, name)
        {
        }


        // Public methods:

        // EFFECTS: Override method that returns a specific explanation for employee objects.

        public override string explanation()
        {
            string explanation = "\n- our employee " + name + " is not available at this time.";
            return explanation;
        }

    }
}