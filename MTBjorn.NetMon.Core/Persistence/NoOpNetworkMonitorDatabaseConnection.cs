using MTBjorn.NetMon.Core.Network;

namespace MTBjorn.NetMon.Core.Persistence;

public class NoOpNetworkMonitorDatabaseConnection : INetworkMonitorDatabaseConnection
{
	public Task AddResult(PingResult result, Guid requestId) => Task.CompletedTask;

	public Task AddRequest(MonitorRequestInfo requestInfo) => Task.CompletedTask;
}