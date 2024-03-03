# Network Monitor

A network monitoring tool.

## Hosting in Docker (ARM32v7)

To host on Linux with the ARM32v7 architecture, such as on a remote Raspberry Pi 3 Model B...

```powershell
# Build image
docker build -t netmon/v0.1-arm32v7 -f .\arm32v7.Dockerfile .

# Save image as a tarball
docker save -o "C:\image-netmon-v0.1-arm32v7.tar" netmon/v0.1-arm32v7

# Upload image over Putty's SSH file transfer
PSCP.EXE "C:\image-netmon-v0.1-arm32v7.tar" user@SERVER:Downloads/image-netmon-v0.1-arm32v7.tar

# Load image into Docker (on remote device)
docker load -i Downloads/image-netmon-v0.1-arm32v7.tar

# Create container and map local port 7129 to container port 80
docker create --name netmon -p 7129:80 netmon/v0.1-arm32v7

# Start container
docker start netmon
```

NOTE: set the db file path in the `appsettings.json` configuration according to the directory structure within Docker -- e.g., `/app/_network-monitor.db`

## Hosting in Docker (x64)

To host on Linux with the x64 architecture...

```powershell
# Build image
docker build -t netmon/v0.1-x64 -f .\linux-x64.Dockerfile .

# Create container and map local port 7129 to container port 80
docker create --name netmon -p 7129:80 netmon/v0.1-x64

# Start, and then access via http://localhost:7129
docker start netmon
```

## Further handling data

An example:
```powershell
# Get all monitoring request records
Invoke-SqliteQuery -Query "SELECT * FROM MonitorRequest" -DataSource "C:\Downloads\_network-monitor.db"

# Get all ping results
Invoke-SqliteQuery -Query "SELECT * FROM PingResults" -DataSource "C:\Downloads\_network-monitor.db"
```