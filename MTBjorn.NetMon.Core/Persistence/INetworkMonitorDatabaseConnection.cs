using MTBjorn.NetMon.Core.Network;

namespace MTBjorn.NetMon.Core.Persistence;

public interface INetworkMonitorDatabaseConnection
{
	Task AddResult(PingResult result, Guid requestId);

	Task AddRequest(MonitorRequestInfo requestInfo);
}