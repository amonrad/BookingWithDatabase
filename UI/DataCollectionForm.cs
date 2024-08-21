using System.Globalization;

namespace BookingWithDatabase
{
    public class DataCollectionForm : IDataCollectionForm
    {
        // Fields:

        private TimeSpan openingTime = Factory.createNewTimeSpan(10, 0, 0); // at 10 AM
        private TimeSpan closingTime = Factory.createNewTimeSpan(14, 0, 0); // at 14 AM
        private string openingTimeString;
        private string closingTimeString;
        private int possibleDaySpan = 60; // number of days for booking to be within
        private int[] possibleDurations = { (int)MeetingDuration.min15, (int)MeetingDuration.min30, (int)MeetingDuration.min45, (int)MeetingDuration.min60 };

        // Constructor:

        public DataCollectionForm()
        {
            openingTimeString = openingTime.ToString(@"hh\:mm");
            closingTimeString = closingTime.ToString(@"hh\:mm");
        }

        // Public Methods:

        // REQUIRES: List of existing clients.
        // MODIFIES: Adds a new client to the list of existing clients if the entered name is not found in the list.
        // EFFECTS: Requests the user to input their name and handles the creation of a new client or reuse of an existing client based on the input.

        public Bookable GetClientInput(List<Bookable> existingClients, IAppDbContext dbContext)
        {
            string enteredName;

            // Perform a loop to handle user input until a client is selected from or added to the list.

            do
            {
                Console.Clear();

                // Print prompt for user to enter their name.
                Console.Write("Enter your name: ");

                enteredName = Console.ReadLine();

                // Validate input
                if (!string.IsNullOrWhiteSpace(enteredName))
                {
                    // Check if a client with the entered name already exists in the database
                    var existingClient = dbContext.Clients.FirstOrDefault(c => c.name.ToLower() == enteredName.ToLower());

                    if (existingClient != null)
                    {
                        // If customer exists, print welcome message and return existing customer.
                        Console.Clear();
                        Console.WriteLine($"Dear {enteredName}, welcome back. Press any key to continue.");
                        Console.ReadKey();
                        return existingClient;
                    }
                    else
                    {
                        // If the client does not exist, create a new client and add it to the database
                        var newClient = Factory.CreateClient(Guid.NewGuid(), enteredName);
                        dbContext.Clients.Add(newClient);
                        dbContext.SaveChanges(); // Save changes to the database
                        Console.Clear();
                        Console.WriteLine($"Dear {enteredName}, welcome. Press any key to continue.");
                        Console.ReadKey();
                        return newClient;
                    }
                }
                else
                {
                    // print error message if input is empty or only consists of spaces.
                    Console.WriteLine("You did not enter anything. Press any key to try again.");
                    Console.ReadKey();
                }

            } while (true); // Repeat until a valid client is returned.
        }

        // EFFECTS: print prompt in console which lets user enter date & time. Return date if valid.

        public DateTime getDateInput()
        {
            DateTime userDateTime;

            // Execute loop handling user input until valid DateTime is selected
            while (true)
            {
                // Convert to strings to use open and close time in prompt
                string formattedOpeningTime = openingTime.ToString(@"hh\:mm");
                string formattedClosingTime = closingTime.ToString(@"hh\:mm");

                Console.Clear();

                // Prints a prompt with instructions to the user about correctly entering the desired date and time.
                Console.WriteLine($"Enter a date within the next {possibleDaySpan} days and a time for your desired meeting.");
                Console.WriteLine($"On a weekday, and between {formattedOpeningTime}-{formattedClosingTime}");
                Console.WriteLine("(e.g., 'yyyy-MM-dd TT:mm').");
                Console.WriteLine("And note that the minute value must be either 00, 15, 30, or 45:");

                string userInput = Console.ReadLine();

                // check if input is a valid DateTime
                if (DateTime.TryParse(userInput, out userDateTime) && IsDateTimeValid(userDateTime))
                {
                    // Convert the entered DateTime to UTC
                    if (userDateTime.Kind != DateTimeKind.Utc)
                    {
                        userDateTime = userDateTime.ToUniversalTime();
                    }
    
                    // Return chosen DateTime, if valid.
                    return userDateTime;
                }
                else
                {
                    // if chosen DateTime is not valid, print error message.
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }


        // EFFECTS: print prompt in console which lets user enter duration. Return duration if valid.

        public int getDurationInput()
        {
            int duration;

            // Perform loop to handle user input until valid duration is chosen
            while (true)
            {
                // Clear console window.
                Console.Clear();

                // Print prompt for user to enter desired duration based on options (available durations).
                Console.WriteLine($"Enter the desired duration for the meeting, either {string.Join(", ", possibleDurations)}");

                string userInput = Console.ReadLine();

                // check if input is a valid duration.
                if (
                    int.TryParse(userInput, out duration) && IsDurationValid(duration))
                {
                    // Return chosen duration, if valid.
                    return duration;
                }
                else
                {
                    // If chosen duration is not valid, print error message.
                    Console.WriteLine("You entered an invalid duration. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        // REQUIRES: list of available employees.
        // EFFECTS: print prompt in console which lets user enter an employee. If the employee is among the list of available employees, return the employee.

        public Bookable getEmployeeInput(List<Bookable> employees)
        {
            string enteredName;

            // Perform loop to handle user input until valid employee is chosen
            while (true)
            {
                // Clear console window.
                Console.Clear();

                // Print overview of available employees.
                Console.WriteLine("We have the following employees: \n");
                foreach (Bookable employee in employees)
                {
                    Console.WriteLine(employee.name);
                }

                // Print prompt for user to enter desired employee.
                Console.Write("\nWho do you want to meet with: ");

                enteredName = Console.ReadLine();

                // Store user-entered employee.
                Bookable selectedEmployee = getEmployeeByName(enteredName, employees);

                // Call helper function to find selected employee in list of available employees.
                if (selectedEmployee != null)
                {
                    // Return selected employee, if valid.
                    return selectedEmployee;
                }
                else
                {
                    // If selected employee is not valid, print error message.
                    Console.WriteLine($"{enteredName} is not one of our employees. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        // REQUIRES: list of available locations.
        // EFFECTS: print prompt in console which lets user enter a location. If the location is among the list of available locations, return the location.

        public Bookable getLocationInput(List<Bookable> locations)
        {
            string enteredLocation;

            // Perform loop to handle user input until valid location is chosen
            while (true)
            {
                // Clear console window.
                Console.Clear();

                // Print overview of available locations.
                Console.WriteLine("We have the following locations: \n");
                foreach (Bookable location in locations)
                {
                    Console.WriteLine(location.name);
                }

                // Print prompt for user to enter desired location.
                Console.Write("\nWhere do you want the meeting to take place: ");

                // Store user-entered location.
                enteredLocation = Console.ReadLine();

                // Call helper function to find the selected location in list of available locations.
                Bookable selectedLocation = getLocationByName(enteredLocation, locations);

                if (selectedLocation != null)
                {
                    // Return the selected location, if valid.
                    return selectedLocation;
                }
                else
                {
                    // If selected location is not valid, print error message.
                    Console.WriteLine($"{enteredLocation} is not one of our locations. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }

        // REQUIRES: A non-empty list of Generics.
        // EFFECTS: If input is valid, generate a random index and return corresponding element from the list.

        public T GetRandomElement<T>(List<T> list)
        {
            // Check if the list is null or empty, if it is, throw an exception
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("The list is either null or empty.");
            }

            Random random = Factory.createNewRandom();

            // Generate random index from array
            int randomIndex = random.Next(list.Count);

            // Return element from random index in array
            return list[randomIndex];
        }

        // EFFECTS: generate and return a random DateTime within specified bounds

        public DateTime GetRandomDateTime()
        {
            Random random = Factory.createNewRandom();

            // Generate random date within specified time horizon
            DateTime startDate = DateTime.Now.AddDays(1);
            DateTime endDate = DateTime.Now.AddDays(possibleDaySpan);
            int range = (endDate - startDate).Days;
            DateTime randomDate = startDate.AddDays(random.Next(range));

            // Check that the date is not Saturday or Sunday
            while (randomDate.DayOfWeek == DayOfWeek.Saturday || randomDate.DayOfWeek == DayOfWeek.Sunday)
            {
                randomDate = randomDate.AddDays(1);
            }

            // Generate random time with minutes 00, 15, 30, or 45
            int hour = random.Next(openingTime.Hours, closingTime.Hours + 1);
            int minute = random.Next(0, 4) * 15; // 0, 1, 2, or 3 * 15
            int second = random.Next(0, 60);

            // create new DateTime with generated parameters
            DateTime randomDateTime = Factory.createNewDateTime(randomDate.Year, randomDate.Month, randomDate.Day, hour, minute, second);

            // Convert the entered DateTime to UTC
            if (randomDateTime.Kind != DateTimeKind.Utc)
            {
                randomDateTime = randomDateTime.ToUniversalTime();
            }

            return randomDateTime;
        }

        // EFFECTS: Generate and return a random duration from array of possible durations

        public int GetRandomDuration()
        {
            Random random = Factory.createNewRandom();

            // generate random index from array
            int randomIndex = random.Next(possibleDurations.Length);

            // get random duration from array
            int randomDuration = possibleDurations[randomIndex];

            return randomDuration;
        }

        // Private Methods:

        // REQUIRES: A DateTime must be provided as input.
        // EFFECTS: Performs a series of validations on the given DateTime and returns true only if all validations are true. Print error message for failed validation.

        private bool IsDateTimeValid(DateTime dateTime)
        {
            // check if dateTime is in the future
            if (dateTime <= DateTime.Now)
            {
                Console.WriteLine("The selected time must be in the future.");
                return false;
            }

            // check if dateTime is within specified time horizon
            if (dateTime > DateTime.Now.AddDays(possibleDaySpan))
            {
                Console.WriteLine($"The selected time must be within the next {possibleDaySpan} days.");
                return false;
            }

            // check that dateTime is not Saturday or Sunday
            if (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.WriteLine("The selected day must not be a Saturday or Sunday.");
                return false;
            }

            // check if dateTime is within opening hours
            if (dateTime.TimeOfDay < openingTime || dateTime.TimeOfDay > closingTime)
            {
                Console.WriteLine($"The selected time must be between {openingTimeString}-{closingTimeString}). Please try again.");
                return false;
            }

            // check if minutes are 0, 15, 30, or 45
            if (dateTime.Minute != 0 && dateTime.Minute != 15 && dateTime.Minute != 30 && dateTime.Minute != 45)
            {
                Console.WriteLine("The selected minutes must be either 00, 15, 30, or 45. Please try again.");
                return false;
            }

            return true; // all validations pass
        }

        // REQUIRES: int as input.
        // EFFECTS: return true if input-int is among possibleDurations. Otherwise, false.

        private bool IsDurationValid(int number)
        {
            // Check if the number is one of the valid durations
            return possibleDurations.Contains(number);
        }

        // REQUIRES: A string and a list of employees.
        // EFFECTS: return employee from list if the name matches the string. Return null if no match.

        private Bookable getEmployeeByName(string name, List<Bookable> employees)
        {
            foreach (Bookable employee in employees)
            {
                if (employee.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return employee;
                }
            }
            return null;
        }

        // REQUIRES: A string and a list of locations.
        // EFFECTS: return location from list if the name matches the string. Return null if no match.

        private Bookable getLocationByName(string nameOfLocation, List<Bookable> locations)
        {
            foreach (Bookable location in locations)
            {
                if (location.name.Equals(nameOfLocation, StringComparison.OrdinalIgnoreCase))
                {
                    return location;
                }
            }
            return null;
        }
    }
}