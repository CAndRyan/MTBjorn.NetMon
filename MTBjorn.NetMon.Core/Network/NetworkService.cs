using MTBjorn.NetMon.Core.Persistence;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace MTBjorn.NetMon.Core.Network;

public class NetworkService
{
	private readonly ConcurrentBag<PingResult> responses = new ConcurrentBag<PingResult>();
	private readonly INetworkMonitorDatabaseConnection databaseConnection;

	public NetworkService(INetworkMonitorDatabaseConnection databaseConnection)
	{
		this.databaseConnection = databaseConnection;
	}

	public PingResult[] CurrentResponses => responses.OrderBy(r => r.Timestamp).ToArray();

	public async Task Monitor(MonitorRequestInfo monitorInfo)
	{
		var (window, ipAddress, resolution, requestTimeout) = monitorInfo;

		if (window.TotalMilliseconds <= 0 || window.TotalMilliseconds > int.MaxValue)
			throw new ArgumentOutOfRangeException(nameof(monitorInfo.Window), "Window, in ms, must be a positive int32 value. Max is ~24 days");

		if (requestTimeout <= 0)
			throw new ArgumentOutOfRangeException(nameof(monitorInfo.RequestTimeout), "Request timeout, in ms, must be a positive integer");

		if (resolution < requestTimeout)
			throw new ArgumentOutOfRangeException(nameof(monitorInfo.Resolution), "Resolution, in ms, must be greater than the request timeout");

		responses.Clear();

		var ipAddressObj = IPAddress.Parse(ipAddress);
		var windowMs = (int)window.TotalMilliseconds;

		await databaseConnection.AddRequest(monitorInfo);

		var monitorStopwatch = Stopwatch.StartNew();

		do
		{
			var pingStopwatch = Stopwatch.StartNew();
			var result = await Ping(ipAddressObj, requestTimeout);
			await SaveResult(result, monitorInfo.Id);

			pingStopwatch.Stop();
			await Sleep(resolution, (int)pingStopwatch.ElapsedMilliseconds);
		} while (monitorStopwatch.ElapsedMilliseconds < windowMs);

		monitorStopwatch.Stop();
	}

	public async Task<MonitorRequestInfo[]> GetRequests() => await databaseConnection.GetRequests();

	private async Task SaveResult(PingResult result, Guid requestId)
	{
        responses.Add(result);

        await databaseConnection.AddResult(result, requestId);
    }

	private static Task Sleep(int resolution, int requestElapsedMs)
	{
        var sleepTime = resolution - requestElapsedMs;
        if (sleepTime > 0)
            Thread.Sleep(sleepTime);

		return Task.CompletedTask;
    }

	private static async Task<PingResult> Ping(IPAddress ipAddress, int requestTimeout)
	{
		var buffer = GetRequestBuffer();
		var options = new PingOptions(64, true);
		var pinger = new Ping();

		var timestamp = DateTime.Now;
		var result = await pinger.SendPingAsync(ipAddress, requestTimeout, buffer, options);

		var latency = result.Status is IPStatus.Success ? (int?)result.RoundtripTime : null;

		return new PingResult(timestamp, latency);
	}

	private static byte[] GetRequestBuffer(int byteSize = 32)
	{
		var data = string.Join(string.Empty, Enumerable.Repeat('a', byteSize));
		return Encoding.ASCII.GetBytes(data);
	}
}
