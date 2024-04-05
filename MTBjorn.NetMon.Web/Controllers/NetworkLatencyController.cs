using Microsoft.AspNetCore.Mvc;
using MTBjorn.NetMon.Core.Network;
using MTBjorn.NetMon.Web.Services;

namespace MTBjorn.NetMon.Web.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class NetworkLatencyController : ControllerBase
{
	private readonly NetworkServiceDriver driver;

	private readonly ILogger<NetworkLatencyController> _logger;

	public NetworkLatencyController(ILogger<NetworkLatencyController> logger, NetworkServiceDriver driver)
	{
		_logger = logger;
		this.driver = driver;
	}

	[HttpPost]
	public async Task Invoke(int windowInMinutes = 1)
	{
		await driver.Invoke(windowInMinutes);
	}

	[HttpGet]
	public bool IsMonitorActive() => driver.IsMonitorActive;

	[HttpGet]
	public IEnumerable<PingResult> GetActiveResults() => driver.GetResults();

	[HttpGet]
	public async Task<MonitorRequestInfo[]> GetRequests() => await driver.GetRequests();
}
