namespace MTBjorn.NetMon.Core.Persistence;

public static class DatabaseConnectionFactory
{
	public static async Task<INetworkMonitorDatabaseConnection> GetConnection(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
			return new NoOpNetworkMonitorDatabaseConnection();

		if (!File.Exists(filePath))
			await new CreateNetworkMonitorDatabaseConnection(filePath).CreateDatabase();

		return new NetworkMonitorDatabaseConnection(filePath);
	}
}