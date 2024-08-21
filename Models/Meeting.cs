using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingWithDatabase
{
    public class Meeting : IMeeting
    {
        // fields:

        [Key]
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid LocationId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Bookable Client { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Bookable Employee { get; set; }

        [ForeignKey("LocationId")]
        public virtual Bookable Location { get; set; }

        public DateTime Start { get; set; }
        public int Duration { get; set; }
        public DateTime End => Start.AddMinutes(Duration);

        [NotMapped]
        public string date { get; }

        [NotMapped]
        public string timeSlot { get; }

        [NotMapped]
        private string rejection;

        [NotMapped]
        public bool isPossible { get; set ; } 

        // Constructors:

        public Meeting() // Parameterless constructor required by EF Core
        {
            Id = Guid.NewGuid();
        }

        public Meeting(Bookable client, DateTime dateTime, int duration, Bookable employee, Bookable location)
            : this() // Call the parameterless constructor to ensure Id is generated
        {
            Client = client;
            Start = dateTime;
            Duration = duration;
            Employee = employee;
            Location = location;

            date = Start.ToString("dd/MM-yyyy");
            timeSlot = Start.ToString("HH:mm") + "-" + Start.AddMinutes(duration).ToString("HH:mm");

            rejection = "Dear " + client.name + " - Unfortunately, it is not possible to book your requested meeting on " + date + " at " + timeSlot + " because: ";
        }

        // Public methods:

        // EFFECTS: Returns confirmation for booked meeting with client's name, date, time, location, and employee's name.
        public string meetingConfirmation()
        {
            // Composes string based on meeting information.
            string meetingConfirmation = "Dear " + Client.name + " - You have successfully booked a meeting on " + date + " at " + timeSlot + " at " + Location.name + ". You will be greeted by our friendly employee " + Employee.name + ".";

            return meetingConfirmation;
        }

        // EFFECTS: Returns reason ('rejection') for rejecting the meeting.
        public string meetingRejection()
        {
            return rejection;
        }
        
        // MODIFIES: Changes 'rejection'
        // EFFECTS: add specified string to 'rejection'
        public void addRejectionCause(string cause)
        {
            rejection += cause;
        }
    }
}