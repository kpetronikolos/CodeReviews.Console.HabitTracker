using HabitTrackerLibrary;
using System.Globalization;

namespace HabitTracker;

public static class UserInputHandler
{
    public static void GetUserInput(SqliteCrud sql)
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("------------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    HabitTrackerHandler.GetAllRecords(sql);
                    break;
                case "2":
                    HabitTrackerHandler.Insert(sql);
                    break;
                case "3":
                    HabitTrackerHandler.Delete(sql);
                    break;
                case "4":
                    HabitTrackerHandler.Update(sql);
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

    public static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main manu.\n\n");

        string dateInput = Console.ReadLine();

        if (dateInput == "0") return dateInput;

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    public static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
}