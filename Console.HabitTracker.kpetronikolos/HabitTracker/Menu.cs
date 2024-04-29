using HabitTrackerLibrary;

namespace HabitTracker;

public static class Menu
{
    public static void Init(SqliteCrud sql)
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            DisplayMainMenu();

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    HabitTrackerHandler.InsertHabit(sql);
                    break;
                case "2":
                    InitHabitLogger(sql);
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

    private static void InitHabitLogger(SqliteCrud sql)
    {
        Console.Clear();
        bool continueToMainMenu = false;
        while (continueToMainMenu == false)
        {
            DisplayHabitLoggerMenu();

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    continueToMainMenu = true;
                    break;
                case "1":
                    HabitTrackerHandler.GetAllHabits(sql);
                    break;
                case "2":
                    HabitTrackerHandler.GetHabitLog(sql);
                    break;
                case "3":
                    HabitTrackerHandler.InsertHabitLog(sql);
                    break;
                case "4":
                    HabitTrackerHandler.DeleteHabitLog(sql);
                    break;
                case "5":
                    HabitTrackerHandler.UpdateHabitLog(sql);
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 5.\n");
                    break;
            }
        }
    }

    private static void DisplayMainMenu()
    {
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close Application.");
        Console.WriteLine("Type 1 to Create a new Habit.");
        Console.WriteLine("Type 2 to Open the Habit Logger.");
        Console.WriteLine("------------------------------------------\n");
    }

    private static void DisplayHabitLoggerMenu()
    {
        Console.WriteLine("\n\nHABIT LOGGER MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Return to Main Menu.");
        Console.WriteLine("Type 1 to View All Habits.");
        Console.WriteLine("Type 2 to View Habit Logs.");
        Console.WriteLine("Type 3 to Insert Record.");
        Console.WriteLine("Type 4 to Delete Record.");
        Console.WriteLine("Type 5 to Update Record.");
        Console.WriteLine("------------------------------------------\n");
    }
}
