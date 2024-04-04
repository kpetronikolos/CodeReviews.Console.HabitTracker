using HabitTrackerLibrary;
using HabitTracker;

SqliteCrud sql = new(ConnectionHandler.GetConnectionString());

HabitTrackerHandler.CreateTable(sql);

UserInputHandler.GetUserInput(sql);