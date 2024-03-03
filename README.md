# Network Monitor

A network monitoring tool.

## Hosting in Docker

An example:
```powershell
docker build -t netmon/v0.1 .

# Create container and map local port 7129 to container port 80
docker create --name netmon -p 7129:80 netmon/v0.1

docker start netmon
# Access via http://localhost:7129

# or create tar ball to install on remote system
docker save -o C:\image-netmon-v0.1.tar netmon/v0.1
# and send to remote server
.\putty\PSCP.EXE -i .\private-key.ppk -P 22 C:\image-netmon-v0.1.tar user@SERVER:Downloads/image-netmon-v0.1.tar
# and load into docker on remote server
docker load -i Downloads/image-netmon-v0.1.tar
```

NOTE: set the db file path according to the directory structure within Docker -- e.g., `/app/_network-monitor.db`

## Further handling data

An example:
```powershell
# Get all monitoring request records
Invoke-SqliteQuery -Query "SELECT * FROM MonitorRequest" -DataSource "C:\Downloads\_network-monitor.db"

# Get all ping results
Invoke-SqliteQuery -Query "SELECT * FROM PingResults" -DataSource "C:\Downloads\_network-monitor.db"
```