namespace HabitTrackerLibrary;

public static class SeedDataHelper
{
    public static void SeedData(SqliteCrud sql)
    {
        if (DataAlreadySeeded(sql)) return;

        sql.SeedHabitsData();

        var habits = sql.GetAllHabitRecords();

        sql.SeedRecordsData(habits, numberOfRecords:10);
    }

    private static bool DataAlreadySeeded(SqliteCrud sql)
    {
        return sql.GetHabitRecordsCount() > 0;
    }
}
