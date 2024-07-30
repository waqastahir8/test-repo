
## Environment Variables configuration

### Configuration to read key vault values

These settings are found under `app-userservice-dev-001 | Environment variables`

A key vault is needed in order to read secure configuration items, the follow variable needs to be configured for the service:

```
KeyVaultOptions__KeyVaultUri
KeyVaultOptions__TenantId
KeyVaultOptions__ClientId
KeyVaultOptions__ClientSecret
APPLICATIONINSIGHTS_CONNECTION_STRING
Copy the value from app service and paste in the appsettings.Development.json

"KeyVaultOptions": {

  "KeyVaultUri": "paste keyVault Uri here",
  "TenantId": "paste keyVault Uri here",
  "ClientId": "paste ClientId here"
  "ClientSecret" : "paste Client Secret here"
}
"ApplicationInsights": {
    "ConnectionString": "paste application insight connection string"
  }

```

# Development Environment configuration
### Install the DotNet CLI from microsoft

https://dotnet.microsoft.com/download/dotnet

### Make sure to set your github environment variables:

```sh
export GITHUB_FT_TOKEN=#your github token
```
```sh
export GITHUB_FT_USERNAME=#your github user name
```

### add the Nuget source in case you need it outside the development environment:
```sh
dotnet nuget add source --username $GITHUB_FT_USERNAME --password $GITHUB_FT_TOKEN --store-password-in-clear-text --name github "https://nuget.pkg.github.com/free-alliance/index.json"
```


# Compile the solution

```sh
export AZURE_WEBAPP_NAME=app-userservice-dev-001
export AZURE_WEBAPP_PACKAGE_PATH='./publish'
export SOLUTION_FILE=./AmeriCorpsUsers.sln
export API_PROJECT=./AmeriCorps.Users.Api
export MIGRATION_PROJECT=cd 
```

```sh
dotnet nuget update source github --username $GITHUB_FT_USERNAME --password $GITHUB_FT_TOKEN --store-password-in-clear-text

dotnet restore $SOLUTION_FILE

dotnet publish $PI_PROJECT --configuration Release --no-restore --output $AZURE_WEBAPP_PACKAGE_PATH

dotnet run
```



# For Local Development, do the following steps

     1. create appsettings.local.json file in Americorp.Users.Api

     2. Copy paste the following settings


```

{
  "KeyVaultOptions": {
    "KeyVaultUri": "paste keyVault Uri here",
    "TenantId": "paste keyVault Uri here",
    "ClientId": "paste ClientId here"
    "ClientSecret" : "paste Client Secret here"
  },
  "UserContextOptions": {
    "DefaultConnectionString": "Server=localhost;Port=5432;Database=citus;Username=citus;Password=<password_here>"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Debug"
      }
    }
  },
  "ApplicationInsights": {
    "ConnectionString": "paste application insight connection string"
  }
}

```

# Run the migration if needed in your local database

```sh
cd $MIGRATION_PROJECT

dotnet tool install --global dotnet-ef

dotnet ef database update 
```