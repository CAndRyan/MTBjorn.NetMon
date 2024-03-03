namespace MTBjorn.NetMon.Core.Persistence;

internal class CreateNetworkMonitorDatabaseConnection : DatabaseConnection
{
	public CreateNetworkMonitorDatabaseConnection(string filePath) : base(filePath) { }

	public async Task CreateDatabase()
	{
		await CreateMonitorRequestTable();
		await CreatePingResultsTable();
	}

	private async Task CreatePingResultsTable()
	{
		await Execute((command) =>
		{
			command.CommandText = @"
CREATE TABLE PingResults (
	Id INTEGER PRIMARY KEY,
	RequestId UNIQUEIDENTIFIER NOT NULL,
	Timestamp DATETIME NOT NULL,
	Latency INTEGER,
	FOREIGN KEY(RequestId) REFERENCES MonitorRequest(Id)
);
";
		});
	}

	private async Task CreateMonitorRequestTable()
	{
		await Execute((command) =>
		{
			command.CommandText = @"
CREATE TABLE MonitorRequest (
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	StartTime DATETIME NOT NULL,
	Window INTEGER NOT NULL,
	IpAddress TEXT NOT NULL,
	Resolution INTEGER NOT NULL,
	RequestTimeout INTEGER NOT NULL
);
";
		});
	}
}