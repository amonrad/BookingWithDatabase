# BookingWithDatabase


## Overview

- **BookingSimpleConsoleApp** is a console-based application designed for managing bookings, including meeting scheduling and client management. This version integrates Entity Framework Core with PostgreSQL to handle data persistence and management. The application follows an object-oriented approach to efficiently manage various aspects of booking.


## Features

- **Bookable Entities**: Manage different types of bookable resources (e.g., locations, employees and clients).
- **Meeting Scheduling**: Schedule and manage meetings, including duration and participants.
- **Display Management**: Handle the display of information to the user.


## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or higher recommended)
- [Visual Studio Code](https://code.visualstudio.com/) (or any other code editor)
- [PostgreSQL](https://www.postgresql.org/download/) (Ensure PostgreSQL is installed and running)
- [Entity Framework Core Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) (for managing migrations and database updates)

### Installation

1. **Clone the repository**:
```bash
git clone https://github.com/yourusername/BookingSimpleConsoleApp.git
```

2. **Navigate to the project directory**:
```bash
cd BookingSimpleConsoleApp
```

3. **Restore the project dependencies**:
```bash
dotnet restore
```

4. **Install EF Core and Npgsql (PostgreSQL provider)**:
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

5. **Set up the PostgreSQL database**:
- Ensure your PostgreSQL server is running and create a database for your application.
- Update the connection string in the appsettings.json file to point to your PostgreSQL database.

6. **Apply migrations to create the database schema**:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```


## Usage
1. **Build the project**:
```bash
dotnet build
```

2. **Run the application**:
```bash
dotnet run
```


## Project Structure

### Models
- **AppDbContext.cs**: Entity Framework Core DbContext class for interacting with the PostgreSQL database.
- **Bookable.cs**: Defines the base class for bookable entities.
- **Client.cs**: Defines the client entity and its properties.
- **Employee.cs**: Defines the employee entity and its properties.
- **Location.cs**: Defines the location entity and its properties.
- **Meeting.cs**: Defines the meeting entity and its properties.

### UI
- **DataCollectionForm.cs**: Form for collecting and managing data input.
- **Display.cs**: Manages the display of information to the user.
- **HandleBooking.cs**: Handles various booking operations.
- **MeetingDuration.cs**: Enum that defines the duration of meetings.
- **NewBookingsForm.cs**: Form for creating new bookings.
- **Reception.cs**: Manages the booking reception and processing.

### Root Folder
- **Program.cs**: Entry point of the application.
- **factory.cs**: Contains factory classes for creating instances of different entities.
- **CleanUpPastMeetings.cs**: Contains logic to clean up past meetings during setup.

- **Interfaces**: Various interfaces for implementing the core functionality.


## License
This project is licensed under the MIT License.
	
