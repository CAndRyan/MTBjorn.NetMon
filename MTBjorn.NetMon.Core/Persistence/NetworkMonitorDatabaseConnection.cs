using MTBjorn.NetMon.Core.Network;

namespace MTBjorn.NetMon.Core.Persistence;

public class NetworkMonitorDatabaseConnection : DatabaseConnection, INetworkMonitorDatabaseConnection
{
	public NetworkMonitorDatabaseConnection(string filePath) : base(filePath) { }

	public async Task AddRequest(MonitorRequestInfo requestInfo)
	{
		var startTime = DateTime.Now;

		await Execute((command) =>
		{
			command.CommandText = @"
INSERT INTO MonitorRequest (Id, StartTime, Window, IpAddress, Resolution, RequestTimeout)
VALUES ($id, $startTime, $window, $ipAddress, $resolution, $requestTimeout)
";
			command.Parameters.AddWithValue("$id", requestInfo.Id);
			command.Parameters.AddWithValue("$startTime", startTime);
			command.Parameters.AddWithValue("$window", (int)requestInfo.Window.TotalMilliseconds);
			command.Parameters.AddWithValue("$ipAddress", requestInfo.IpAddress);
			command.Parameters.AddWithValue("$resolution", requestInfo.Resolution);
			command.Parameters.AddWithValue("$requestTimeout", requestInfo.RequestTimeout);
		});
	}

	public async Task AddResult(PingResult result, Guid requestId)
	{
		await Execute((command) =>
		{
			command.CommandText = @"
INSERT INTO PingResults (Timestamp, Latency, RequestId)
VALUES ($timestamp, $latency, $requestId)
";
			command.Parameters.AddWithValue("$timestamp", result.Timestamp);
			command.Parameters.AddWithValue("$latency", result.Latency is null ? DBNull.Value : result.Latency.Value);
			command.Parameters.AddWithValue("$requestId", requestId);
		});
	}
}