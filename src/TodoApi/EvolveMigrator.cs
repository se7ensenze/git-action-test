using System.Reflection;
using Dapper;
using Npgsql;

namespace TodoApi;

public static class EvolveMigrator
{
    private static string _dbtExistSql = @"SELECT true WHERE EXISTS (SELECT FROM pg_database WHERE datname = @DatabaseName);";
    private const string _createDbSql = "CREATE DATABASE {0}";

    public static void MigrateDatabase(string assemblyName,
        string connectionString,
        string defaultDbName,
        bool includeSeedData = false,
        bool dropDatabase = false,
        Action<string>? logDelegate = null)
    {
        var assembly = Assembly.Load(assemblyName);

        // Skip the seed data scripts if we're not in the Development environment
        var filter = $"{assemblyName}.Database" + (includeSeedData ? "" : ".Migrations");

        var databaseNameSegment = connectionString
            .Split(';')
            .FirstOrDefault(i => i.StartsWith("Database="));

        if (databaseNameSegment != null)
        {
            var databaseName = databaseNameSegment.Split('=')[1];
            var postgresDbConnectionString = connectionString.Replace(databaseNameSegment, $"Database={defaultDbName}");
            using var postgresDbConnection = new NpgsqlConnection(postgresDbConnectionString);

            //Check if the database exists and drop it when needed
            if (dropDatabase && postgresDbConnection.QuerySingleOrDefault<bool>(_dbtExistSql, new { DatabaseName = databaseName }))
                postgresDbConnection.Execute($"DROP DATABASE {databaseName} WITH (FORCE);");

            //If the database does not exist, create it
            if (!postgresDbConnection.QuerySingleOrDefault<bool>(_dbtExistSql, new { DatabaseName = databaseName }))
            {
                if (postgresDbConnection.Execute(string.Format(_createDbSql, databaseName)) == 0)
                    throw new Exception($"Could not create database {databaseName} ");
            }
        }

        using var connection = new NpgsqlConnection(connectionString);

        var evolve = new Evolve.Evolve(connection, logDelegate)
        {
            EmbeddedResourceAssemblies = new[] { assembly },
            EmbeddedResourceFilters = new[] { filter },
            IsEraseDisabled = true,
            CommandTimeout = 30,
        };

        evolve.Migrate();
    }
}