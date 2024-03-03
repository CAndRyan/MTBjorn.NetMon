FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -r linux-x64 -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
ENV TZ="America/New_York"
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MTBjorn.NetMon.Web.dll"]

EXPOSE 7129
