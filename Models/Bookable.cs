using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingWithDatabase
{

    public abstract class Bookable : IBookable
    {
        // Fields:

        [Key]
        public Guid? id { get; set; }
        [Column("name")]
        public string name { get; set; }

        // Parameterless constructor required by Entity Framework Core
        protected Bookable()
        {
            id = Guid.NewGuid();
        }

        // Constructor:

        protected Bookable(Guid? id, string name) : this()
        {
            this.id = id;
            this.name = name;
        }

        // Public methods:

        public bool isBookingPossible(IMeeting requestedMeeting, IAppDbContext dbContext)
        {
            bool possible = true;

            // DateTime for validation
            DateTime dateToCheck = requestedMeeting.Start.Date;
            DateTime meetingStart = requestedMeeting.Start;
            DateTime meetingEnd = requestedMeeting.End;

            // Query the database for existing meetings for this bookable
            var existingMeetings = dbContext.Meetings
                .Where(m => m.ClientId == this.id || m.EmployeeId == this.id || m.LocationId == this.id)
                .ToList();

            // Check for overlap with existing meetings
            foreach (var existingMeeting in existingMeetings)
            {
                DateTime existingMeetingStart = existingMeeting.Start;
                DateTime existingMeetingEnd = existingMeeting.End;

                if (isDateTimesOverlapping(requestedMeeting.Start, requestedMeeting.End, existingMeetingStart, existingMeetingEnd))
                {
                    // Return false if the meeting cannot be booked due to overlap with an existing meeting
                    possible = false;
                    break; // No need to continue checking if overlap is found
                }
            }

            // Return true if the meeting can be booked
            return possible;
        }

        public bool isDateTimesOverlapping(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            // Check for non-overlapping cases
            if (end1 <= start2 || end2 <= start1)
            {
                return false;
            }

            // If they are not non-overlapping, they must overlap
            return true;
        }

        public virtual string explanation()
        {
            return "";
        }
    }
}