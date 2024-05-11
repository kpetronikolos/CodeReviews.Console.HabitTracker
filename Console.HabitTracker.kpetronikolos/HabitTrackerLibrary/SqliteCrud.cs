using HabitTrackerLibrary.Models;

namespace HabitTrackerLibrary;

public class SqliteCrud
{
    private readonly string _connectionString;
    private SqliteDataAccess db = new();
    private Random _random;

    public SqliteCrud(string connectionString)
    {
        _connectionString = connectionString;
        _random = new Random();
    }

    public void CreateTables()
    {
        string sql = """
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Unit TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS HabitLogs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER,
                    Quantity INTEGER NOT NULL,
                    Date TEXT NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES Habits(Id)
                );

                """;

        db.SaveData(sql, _connectionString);
    }

    public List<Habit> GetAllHabitRecords()
    {
        string sql = "SELECT * FROM Habits ";

        return db.LoadData<Habit>(sql, _connectionString);
    }

    public void InsertHabitRecord(HabitDTO habit)
    {
        string sql = $"INSERT INTO Habits(Name, Unit) VALUES('{habit.Name}', '{habit.Unit}')";

        db.SaveData(sql, _connectionString);
    }

    public void InsertHabitLogRecord(HabitLogDTO habitLog)
    {
        string sql = $"INSERT INTO HabitLogs(HabitId, Quantity, Date) VALUES({habitLog.HabitId}, {habitLog.Quantity}, '{habitLog.Date}')";

        db.SaveData(sql, _connectionString);
    }

    public int DeleteHabitLogRecord(int id)
    {
        string sql = $"DELETE from HabitLogs WHERE Id = '{id}'";

        return db.DeleteData(sql, _connectionString);
    }

    public void UpdateHabitLogRecord(HabitLogDTO habitLog, int id)
    {
        string sql = $"UPDATE HabitLogs SET Quantity = {habitLog.Quantity}, Date = '{habitLog.Date}'  WHERE Id = {id}";

        db.SaveData(sql, _connectionString);
    }

    public List<HabitLog> GetAllHabitLogRecordsByHabitId(int id)
    {
        string sql = @$" SELECT hl.Id, hl.Quantity, hl.Date
                        FROM Habits h 
                        INNER JOIN HabitLogs hl
                            ON h.Id = hl.HabitId
                        WHERE h.Id = {id} ";

        return db.LoadData<HabitLog>(sql, _connectionString);
    }

    public bool DoesHabitRecordExist(int id)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM Habits WHERE Id = {id})";

        return db.ExistsInTable(sql, _connectionString);
    }

    public bool DoesHabitLogRecordExist(int id)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM HabitLogs WHERE Id = {id})";

        return db.ExistsInTable(sql, _connectionString);
    }

    public void SeedHabitsData()
    {
        string sql = """
            
                -- Seed data for the Habits Table
                INSERT INTO Habits (Name, Unit) VALUES ('Drinking Water', 'Glasses');
                INSERT INTO Habits (Name, Unit) VALUES ('Pushups', 'Reps');
                INSERT INTO Habits (Name, Unit) VALUES ('Running', 'Meters');
                INSERT INTO Habits (Name, Unit) VALUES ('Walking', 'Number Of Steps');
                INSERT INTO Habits (Name, Unit) VALUES ('Coding', 'Minutes');
                INSERT INTO Habits (Name, Unit) VALUES ('Reading', 'Pages');
                INSERT INTO Habits (Name, Unit) VALUES ('Sleeping', 'Hours');
                INSERT INTO Habits (Name, Unit) VALUES ('Learn Portuguese', 'New Words');
                INSERT INTO Habits (Name, Unit) VALUES ('Saving Money', 'Euros');
                INSERT INTO Habits (Name, Unit) VALUES ('Meditation', 'Minutes');
            """;

        db.SaveData(sql, _connectionString);
    }

    public void SeedRecordsData(List<Habit> habits, int numberOfRecords)
    {
        string sql = "";
        foreach (var habit in habits)
        {
            for (int i = 0; i < numberOfRecords; i++)
            {
                int randomValue = GenerateRandomValue(100);
                sql += $"INSERT INTO HabitLogs (HabitId, Quantity, Date) VALUES ({habit.Id}, {randomValue}, '{DateTimeHelpers.NextWeeksDates()[i]}'); ";
            }
        }

        db.SaveData(sql, _connectionString);
    }

    public int GetHabitRecordsCount()
    {
        string sql = $"SELECT COUNT(*) FROM Habits";

        return db.GetIntData(sql, _connectionString);
    }

    private int GenerateRandomValue(int maxValue)
    {
        return _random.Next(maxValue);
    }
}