using HabitTrackerLibrary;
using HabitTracker;

SqliteCrud sql = new(ConnectionHandler.GetConnectionString());

HabitTrackerHandler.CreateTables(sql);

Menu.Init(sql);