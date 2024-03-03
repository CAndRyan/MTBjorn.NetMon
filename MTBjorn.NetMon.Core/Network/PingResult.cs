namespace MTBjorn.NetMon.Core.Network;

public readonly struct PingResult
{
	public PingResult(DateTime timestamp, int? latency)
	{
		Timestamp = timestamp;
		Latency = latency;
	}

	public DateTime Timestamp { get; }

	public int? Latency { get; }
}
