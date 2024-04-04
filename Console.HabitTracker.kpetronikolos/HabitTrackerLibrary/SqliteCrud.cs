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
}