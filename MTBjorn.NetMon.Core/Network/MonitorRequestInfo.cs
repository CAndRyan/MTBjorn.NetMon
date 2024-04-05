namespace MTBjorn.NetMon.Core.Network;

public readonly struct MonitorRequestInfo
{
    private const int defaultResolution = 3000;
    private const int defaultRequestTimeout = 1000;
    private const string googleDnsIpAddress = "8.8.8.8";

	public MonitorRequestInfo(TimeSpan window) : this(Guid.NewGuid(), window, default(DateTime)) { }

    internal MonitorRequestInfo(Guid id, TimeSpan window, DateTime startTime)
    {
        Id = id;
        Window = window;
		StartTime = startTime;
    }

    public Guid Id { get; }
	public TimeSpan Window { get; }
	public string IpAddress { get; init; } = googleDnsIpAddress;
	public int Resolution { get; init; } = defaultResolution;
	public int RequestTimeout { get; init; } = defaultRequestTimeout;
	public DateTime StartTime { get; init; }

	public void Deconstruct(out TimeSpan window, out string ipAddress, out int resolution, out int requestTimeout)
	{
		window = Window;
		ipAddress = IpAddress;
		resolution = Resolution;
		requestTimeout = RequestTimeout;
	}
}
