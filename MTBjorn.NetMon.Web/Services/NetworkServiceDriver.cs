using MTBjorn.NetMon.Core.Network;
using MTBjorn.NetMon.Core.Persistence;

namespace MTBjorn.NetMon.Web.Services;

public class NetworkServiceDriver
{
	private readonly string dbFilePath;

	private NetworkService? activeNetworkService;
	private Task? activeMonitorTask;

	private object activeNetworkServiceLock = new object();
    private object activeMonitorTaskLock = new object();

    public NetworkServiceDriver(string dbFilePath)
    {
		this.dbFilePath = dbFilePath;
    }

	private NetworkService? ActiveNetworkService
	{
		get
		{
			lock (activeNetworkServiceLock)
			{
				return activeNetworkService;
			}
		}
		set
		{
            lock (activeNetworkServiceLock)
            {
                activeNetworkService = value;
            }
        }
	}

    private Task? ActiveMonitorTask
	{
		get
		{
			lock (activeMonitorTaskLock)
			{
				return activeMonitorTask;
			}
		}
		set
		{
            lock (activeMonitorTaskLock)
            {
                activeMonitorTask = value;
            }
        }
	}

	public bool IsMonitorActive
	{
		get
		{
			var task = ActiveMonitorTask;
			return task is not null && !task.IsCompleted;
        }
	}

	public async Task Invoke(int windowInMinutes)
	{
		if (windowInMinutes <= 0)
			throw new ArgumentOutOfRangeException(nameof(windowInMinutes), "Window must be positive");

		if (IsMonitorActive)
			throw new InvalidOperationException("Network monitoring has already been started");

		var monitorRequestInfo = new MonitorRequestInfo(TimeSpan.FromMinutes(windowInMinutes));
		var databaseConnection = await DatabaseConnectionFactory.GetConnection(dbFilePath);
		var service = new NetworkService(databaseConnection);

		ActiveNetworkService = service;
        ActiveMonitorTask = service.Monitor(monitorRequestInfo);
	}

	public IEnumerable<PingResult> GetResults()
	{
		var service = ActiveNetworkService;

		if (service is null)
			return Enumerable.Empty<PingResult>();

		return service.CurrentResponses;
	}

    public async Task<MonitorRequestInfo[]> GetRequests()
	{
		var service = ActiveNetworkService;

		if (service is null)
		{
			var databaseConnection = await DatabaseConnectionFactory.GetConnection(dbFilePath);
			service = new NetworkService(databaseConnection);
			ActiveNetworkService = service;
		}

        return await service.GetRequests();
    }
}
