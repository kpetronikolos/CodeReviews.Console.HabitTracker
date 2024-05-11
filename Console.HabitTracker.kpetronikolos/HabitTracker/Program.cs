using HabitTrackerLibrary;
using HabitTracker;

SqliteCrud sql = new(ConnectionHandler.GetConnectionString());

HabitTrackerHandler.CreateTables(sql);
SeedDataHelper.SeedData(sql);

Menu.Init(sql);