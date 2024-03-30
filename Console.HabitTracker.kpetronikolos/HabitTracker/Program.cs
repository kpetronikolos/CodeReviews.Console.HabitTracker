﻿using Microsoft.Data.Sqlite;
using System.Globalization;

string connectionString = @"Data Source=C:\Study\CSharp\Academy\CodeReviews.Console.HabitTracker\Console.HabitTracker.kpetronikolos\habit-Tracker.db";

using (var connection = new SqliteConnection(connectionString))
{

    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS PushUps (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Reps INTEGER
                        )";

    tableCmd.ExecuteNonQuery();

    connection.Close();

    GetUserInput();
}

void GetUserInput()
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
                GetAllRecords();
                break;
            case "2":
                Insert();
                break;
            case "3":
                Delete();
                break;
            case "4":
                Update();
                break;
            default:
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                break;
        }
    }

}

void GetAllRecords()
{
    Console.Clear();
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            $"SELECT * FROM PushUps ";

        List<PushUp> tableData = new();

        SqliteDataReader reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                new PushUp
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                    Reps = reader.GetInt32(2)
                }); ;
            }
        }
        else
        {
            Console.WriteLine("No rows found");
        }

        connection.Close();

        Console.WriteLine("------------------------------------------\n");
        foreach (var dw in tableData)
        {
            Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Reps: {dw.Reps}");
        }
        Console.WriteLine("------------------------------------------\n");
    }
}

void Insert()
{
    string date = GetDateInput();

    int reps = GetNumberInput("\n\nPlease insert number of reps (no decimals allowed)\n\n");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
           $"INSERT INTO PushUps(date, reps) VALUES('{date}', {reps})";

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }
}

void Delete()
{
    Console.Clear();
    GetAllRecords();

    var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"DELETE from PushUps WHERE Id = '{recordId}'";

        int rowCount = tableCmd.ExecuteNonQuery();

        if (rowCount == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
            Delete();
        }

    }

    Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

    GetUserInput();
}

void Update()
{
    GetAllRecords();

    var recordId = GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM PushUps WHERE Id = {recordId})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
            connection.Close();
            Update();
        }

        string date = GetDateInput();

        int reps = GetNumberInput("\n\nPlease insert number of reps (no decimals allowed)\n\n");

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE PushUps SET date = '{date}', reps = {reps} WHERE Id = {recordId}";

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }


}

string GetDateInput()
{
    Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main manu.\n\n");

    string dateInput = Console.ReadLine();

    if (dateInput == "0") GetUserInput();

    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
        dateInput = Console.ReadLine();
    }

    return dateInput;
}

int GetNumberInput(string message)
{
    Console.WriteLine(message);

    string numberInput = Console.ReadLine();

    if (numberInput == "0") GetUserInput();

    while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
    {
        Console.WriteLine("\n\nInvalid number. Try again.\n\n");
        numberInput = Console.ReadLine();
    }

    int finalInput = Convert.ToInt32(numberInput);

    return finalInput;
}
    

public class PushUp
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Reps { get; set; }
}
