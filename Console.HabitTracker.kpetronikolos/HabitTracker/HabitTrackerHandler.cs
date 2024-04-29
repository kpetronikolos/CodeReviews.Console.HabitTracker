using HabitTrackerLibrary;
using HabitTrackerLibrary.Models;

namespace HabitTracker;

public static class HabitTrackerHandler
{
    private static List<PushUp> pushUps = new List<PushUp>();
    private static List<Habit> habits = new();
    private static List<HabitLog> habitLogs = new();

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

    public static void CreateTables(SqliteCrud sql)
    {
        sql.CreateTables();
    }

    public static void GetAllHabits(SqliteCrud sql)
    {
        Console.Clear();

        habits = sql.GetAllHabitRecords();

        if (habits.Any() == false)
        {
            Console.WriteLine("No habits were found");
            return;
        }

        PrintAllHabitRecords();
    }

    public static void GetHabitLog(SqliteCrud sql)
    {
        PrintAllHabitRecords();

        var recordId = UserInputHandler.GetNumberInput("\n\nPlease type Id of the habit for which logs you would like to display. Type 0 to return to main manu.\n\n");
        if (recordId == 0) return;

        if (sql.DoesHabitRecordExist(recordId) == false)
        {
            Console.WriteLine($"\n\nHabit with Id {recordId} doesn't exist.\n\n");
            GetHabitLog(sql);
            return;
        }

        habitLogs = sql.GetAllHabitLogRecordsByHabitId(recordId);

        if (habitLogs.Any() == false)
        {
            Console.WriteLine("No logs were found for this habit");
            return;
        }

        PrintHabitLogsRecords();
    }

    public static void InsertHabit(SqliteCrud sql)
    {
        string name = UserInputHandler.GetStringInput("\nPlease enter the Habit Name: ");
        string unit = UserInputHandler.GetStringInput("\nPlease enter the Unit of Measurement: ");

        var habit = new HabitDTO
        {
            Name = name,
            Unit = unit,
        };

        sql.InsertHabitRecord(habit);
    }

    public static void InsertHabitLog(SqliteCrud sql)
    {
        PrintAllHabitRecords();

        var recordId = UserInputHandler.GetNumberInput("\n\nPlease type Id of the habit you would like to log. Type 0 to return to main manu.\n\n");
        if (recordId == 0) return;

        if (sql.DoesHabitRecordExist(recordId) == false)
        {
            Console.WriteLine($"\n\nHabit with Id {recordId} doesn't exist.\n\n");
            InsertHabitLog(sql);
            return;
        }

        var habit = GetHabitById(recordId);

        string date = UserInputHandler.GetDateInput();
        if (date == "0") return;

        int quantity = UserInputHandler.GetNumberInput($"\n\nPlease insert number of {habit.Unit} (no decimals allowed)\n\n");

        var habitLog = new HabitLogDTO
        {
            HabitId = recordId,
            Quantity = quantity,
            Date = date
        };

        sql.InsertHabitLogRecord(habitLog);
    }

    public static void DeleteHabitLog(SqliteCrud sql)
    {
        GetHabitLog(sql);

        var recordId = UserInputHandler.GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");
        if (recordId == 0) return;

        int rowCount = sql.DeleteHabitLogRecord(recordId);

        if (rowCount == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
            DeleteHabitLog(sql);
            return;
        }

        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
    }

    public static void UpdateHabitLog(SqliteCrud sql)
    {
        PrintAllHabitRecords();

        var habitId = UserInputHandler.GetNumberInput("\n\nPlease type Id of the habit you would like to update logs. Type 0 to return to main manu.\n\n");
        if (habitId == 0) return;

        if (sql.DoesHabitRecordExist(habitId) == false)
        {
            Console.WriteLine($"\n\nHabit with Id {habitId} doesn't exist.\n\n");
            UpdateHabitLog(sql);
            return;
        }

        var habit = GetHabitById(habitId);

        habitLogs = sql.GetAllHabitLogRecordsByHabitId(habitId);

        if (habitLogs.Any() == false)
        {
            Console.WriteLine("No logs were found for this habit");
            return;
        }

        PrintHabitLogsRecords();

        var recordId = UserInputHandler.GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");
        if (recordId == 0) return;

        if (sql.DoesHabitLogRecordExist(recordId) == false)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
            UpdateHabitLog(sql);
            return;
        }

        string date = UserInputHandler.GetDateInput();
        if (date == "0") return;

        int quantity = UserInputHandler.GetNumberInput($"\n\nPlease insert number of {habit.Unit} (no decimals allowed)\n\n");

        var habitLog = new HabitLogDTO
        {
            HabitId = habitId,
            Quantity = quantity,
            Date = date
        };

        sql.UpdateHabitLogRecord(habitLog, recordId);
    }

    private static void PrintAllHabitRecords()
    {
        Console.Clear();

        Console.WriteLine("------------------------------------------\n");
        foreach (var dw in habits)
        {
            Console.WriteLine($"{dw.Id} - {dw.Name} - {dw.Unit}");
        }
        Console.WriteLine("------------------------------------------\n");
    }

    private static void PrintHabitLogsRecords()
    {
        Console.Clear();

        Console.WriteLine("------------------------------------------\n");
        foreach (var dw in habitLogs)
        {
            Console.WriteLine($"{dw.Id} - {dw.Date} - {dw.Quantity}");
        }
        Console.WriteLine("------------------------------------------\n");
    }

    private static Habit GetHabitById(int id)
    {
        return habits[id-1];
    }
}