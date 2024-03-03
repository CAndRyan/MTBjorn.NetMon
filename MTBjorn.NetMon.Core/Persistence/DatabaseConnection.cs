using Microsoft.Data.Sqlite;

namespace MTBjorn.NetMon.Core.Persistence;

public abstract class DatabaseConnection
{
	private readonly string filePath;

	protected DatabaseConnection(string filePath)
	{
		this.filePath = filePath;
	}

	private string ConnectionString => $"Data Source={filePath}";

	protected async Task Execute(Action<SqliteCommand> setupCommand)
	{
		await using var connection = new SqliteConnection(ConnectionString);
		connection.Open();

		var command = connection.CreateCommand();
		setupCommand(command);

		_ = await command.ExecuteNonQueryAsync();
	}

	protected async Task<T[]> Query<T>(Action<SqliteCommand> setupCommand, Func<SqliteDataReader, T> readRow)
	{
		await using var connection = new SqliteConnection(ConnectionString);
		connection.Open();

		var command = connection.CreateCommand();
		setupCommand(command);

		await using var reader = await command.ExecuteReaderAsync();

		return ReadAll(reader, readRow).ToArray();
	}

	private static IEnumerable<T> ReadAll<T>(SqliteDataReader reader, Func<SqliteDataReader, T> readRow)
	{
		while (reader.Read())
			yield return readRow(reader);
	}
}