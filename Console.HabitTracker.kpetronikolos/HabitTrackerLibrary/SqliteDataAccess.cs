using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTrackerLibrary;

public class SqliteDataAccess
{
    public void SaveData(string sqlStatement, string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = sqlStatement;

            tableCmd.ExecuteNonQuery();
        }
    }

    public List<T> LoadData<T>(string sqlStatement, string connectionString)
    {
        List<T> tableData = new List<T>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = sqlStatement;

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    T rowData = Activator.CreateInstance<T>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        // Assuming T has properties with matching names and types
                        var property = typeof(T).GetProperty(reader.GetName(i));

                        if (property != null && !reader.IsDBNull(i))
                        {
                            if (reader.GetValue(i).GetType() == typeof(Int64))
                            {
                                property.SetValue(rowData, reader.GetInt32(i));
                            }
                            else if (property.PropertyType == typeof(System.DateTime))
                            {
                                property.SetValue(rowData, DateTime.ParseExact(reader.GetString(i), "dd-MM-yy", new CultureInfo("en-US")));
                            }
                            else
                            {
                                property.SetValue(rowData, reader.GetValue(i));
                            }                            
                        }
                    }
                    tableData.Add(rowData);
                }
            }

            return tableData;
        }
    }

    public int DeleteData(string sqlStatement, string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = sqlStatement;

            int rowCount = tableCmd.ExecuteNonQuery();

            return rowCount;
        }
    }

    public bool ExistsInTable(string sqlStatement, string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = sqlStatement;

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            return checkQuery > 0;
        }
    }
}
