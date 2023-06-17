# Overview

Healthz.Api is a C# minimal API showcasing usage of health checks. The API makes usage of:

- Registering a library health check
- Registering a custom health check
- Filter health checks
- Customising output


# Getting Started: 👨🏻‍💻

## Clone: ⚒️

```Powershell
git clone https://github.com/reggieray/dotnet-health-checks
```

## Run: 👟

Run from either two options below:

### Dotnet CLI: 💻

```Powershell
dotnet run --project ./src/Healthz.Api/ --configuration Release
```
### Docker: 🐋

```Powershell
docker-compose up --build
```

### HTTP Documentation: 📄

Explore [HealthzApi.http](HealthzApi.http) and make requests with the following options available:
- Visual Studio: [Visual Studio .http files](https://learn.microsoft.com/en-us/aspnet/core/test/http-files)
- VSCode: [VS Code REST Client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
- CLI: [httpYac](https://github.com/AnWeber/httpyac)

Example usage using httpYac:

```Powershell
httpyac HealthzApi.http
```

## Test: 🧪

```Powershell
dotnet test ./tests/Healthz.Api.Tests/ --configuration Release
```

# Configuration: ⚙️

Alter configuration or environment variables to change the output of the health checks.

### Dotnet CLI: 💻

Alter the `HealthCheck` section within [appsettings.Development.json](src/Healthz.Api/appsettings.Development.json) and re-run.

```json
{
  /*config removed for brevity*/
  "HealthCheck": {
    "MyCustomStartUpHealthCheck": "False",
    "UriCheck": "https://matthewregis.dev/"
  }
}
```

### Docker: 🐋

Alter the `environment` section within [docker-compose.yaml](docker-compose.yaml) and re-run.

```docker
version: "3"
services: 
    healthzapi:
        environment:
            HealthCheck__MyCustomStartUpHealthCheck: "False"
            HealthCheck__UriCheck: "https://matthewregis.dev"
# config removed for brevity #
```