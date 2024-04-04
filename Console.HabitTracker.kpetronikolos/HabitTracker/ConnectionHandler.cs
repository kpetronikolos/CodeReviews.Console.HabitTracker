using Microsoft.Extensions.Configuration;

namespace HabitTracker;

public static class ConnectionHandler
{
    public static string GetConnectionString(string connectionStringName = "Default")
    {
        string output = "";

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var config = builder.Build();

        output = config.GetConnectionString(connectionStringName);

        return output;
    }
}
