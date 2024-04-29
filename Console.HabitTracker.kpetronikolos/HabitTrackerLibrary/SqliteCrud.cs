using HabitTrackerLibrary.Models;

namespace HabitTrackerLibrary;

public class SqliteCrud
{
    private readonly string _connectionString;
    private SqliteDataAccess db = new();

    public SqliteCrud(string connectionString)
    {
        _connectionString = connectionString;
    }    

    public void CreatePushupsTable()
    {
        string sql = @"CREATE TABLE IF NOT EXISTS PushUps (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Reps INTEGER
                        )";
        
        db.SaveData(sql, _connectionString);
    }

    public List<PushUp> GetAllRecords()
    {
        string sql = "SELECT * FROM PushUps ";

        return db.LoadData<PushUp>(sql, _connectionString);
    }

    public void InsertPushupRecord(PushUpDTO pushUp)
    {
        string sql = $"INSERT INTO PushUps(date, reps) VALUES('{pushUp.Date}', {pushUp.Reps})";

        db.SaveData(sql, _connectionString);
    }

    public int DeletePushupRecord(int id)
    {
        string sql = $"DELETE from PushUps WHERE Id = '{id}'";

        return db.DeleteData(sql, _connectionString);
    }

    public void UpdatePushupRecord(PushUpDTO pushUp, int id)
    {
        string sql = $"UPDATE PushUps SET date = '{pushUp.Date}', reps = {pushUp.Reps} WHERE Id = {id}";

        db.SaveData(sql, _connectionString);
    }

    public bool DoesPushupRecordExist(int id)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM PushUps WHERE Id = {id})";

        return db.ExistsInTable(sql, _connectionString);
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
}