using HabitTrackerLibrary;
using HabitTrackerLibrary.Models;

namespace HabitTracker;

public static class HabitTrackerHandler
{
    private static List<PushUp> pushUps = new List<PushUp>();

    public static void CreateTable(SqliteCrud sql)
    {
        sql.CreatePushupsTable();
    }

    public static void GetAllRecords(SqliteCrud sql)
    {
        Console.Clear();

        pushUps = sql.GetAllRecords();

        if (pushUps.Any() == false)
        {
            Console.WriteLine("No records found");
            return;
        }

        PrintAllRecords();
    }

    public static void Insert(SqliteCrud sql)
    {
        string date = UserInputHandler.GetDateInput();
        if (date == "0") return;

        int reps = UserInputHandler.GetNumberInput("\n\nPlease insert number of reps (no decimals allowed)\n\n");

        var pushUp = new PushUpDTO
        {
            Reps = reps,
            Date = date,
        };

        sql.InsertPushupRecord(pushUp);
    }

    public static void Delete(SqliteCrud sql)
    {
        Console.Clear();

        PrintAllRecords();

        var recordId = UserInputHandler.GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");
        if (recordId == 0) return;

        int rowCount = sql.DeletePushupRecord(recordId);

        if (rowCount == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
            Delete(sql);
            return;
        }

        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");        
    }

    public static void Update(SqliteCrud sql)
    {
        PrintAllRecords();

        var recordId = UserInputHandler.GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");
        if (recordId == 0) return;

        if (sql.DoesPushupRecordExist(recordId) == false)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
            Update(sql);
            return;
        }

        string date = UserInputHandler.GetDateInput();
        if (date == "0") return;

        int reps = UserInputHandler.GetNumberInput("\n\nPlease insert number of reps (no decimals allowed)\n\n");

        var pushUp = new PushUpDTO
        {
            Reps = reps,
            Date = date,
        };

        sql.UpdatePushupRecord(pushUp, recordId);
    }

    private static void PrintAllRecords()
    {
        Console.Clear();

        Console.WriteLine("------------------------------------------\n");
        foreach (var dw in pushUps)
        {
            Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Reps: {dw.Reps}");
        }
        Console.WriteLine("------------------------------------------\n");
    }
}