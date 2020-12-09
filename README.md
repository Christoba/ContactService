# Contact Service

A basic contact service supported by:

* An Azure Function App Service
* An Azure CosmosDb Database

And deployed by Azure powershell and GitHub actions

## Azure Functions

Function                   | Trigger     | Description
---------------------------|-------------|----------------------------------------------------------------------------------------------------------
ContactGet                 | GET, POST   | Retrieves a specific contact
ContactsCreate             | POST        | Creates a random set of contacts for a specific user
ContactsDelete             | POST        | Deletes contacts
ContactsGet                | GET, POST   | Retrieves contacts for a specific user
UserGet                    | GET, POST   | Retrieves a specific user
UsersCreate                | POST        | Creates a random set of users
UsersDelete                | POST        | Deletes users
UsersGet                   | GET, POST   | Retrieves users

### Azure Function Authentication

Authentication is done either through a function key or host (all functions) key.
Set a header `x-functions-key` on the request with the value of the key. The key is a secret and can be retrieved from the App Service in the Azure Portal.

### Contacts Create Example

HTTP POST:

```text
https://contact-service-1208.azurewebsites.net/api/ContactsCreate
```

Body:

```json
{
  "UserId": "50449b51-5b81-4dd7-8d33-d46708e1713a",
  "Count": 10
}
```

### Contacts Delete Example

HTTP POST:

```text
https://contact-service-1208.azurewebsites.net/api/ContactsDelete
```

### Contact Get Example

HTTP GET, POST:

```text
https://contact-service-1208.azurewebsites.net/api/ContactGet
```

Body:

```json
{
  "UserId": "30d8ba4c-ac5d-412b-9825-51fbf88db9e7",
  "ContactId": "159a3e4e-374b-4e3f-a643-75a5ea9155b4"
}
```

### Contacts Get Example

HTTP GET, POST:

```text
https://contact-service-1208.azurewebsites.net/api/ContactsGet
```

Body:

```json
{
  "UserId": "50449b51-5b81-4dd7-8d33-d46708e1713a",
  "Skip": 0,
  "Take": 100
}
```

### Users Create Example

HTTP POST:

```text
https://contact-service-1208.azurewebsites.net/api/UsersCreate
```

Body:

```json
10
```

### Users Delete Example

HTTP POST:

```text
https://contact-service-1208.azurewebsites.net/api/UsersDelete
```

### User Get Example

HTTP GET, POST:

```text
https://contact-service-1208.azurewebsites.net/api/UserGet
```

Body:

```json
{
  "UserEmail": "7b1db853-a7cf-4889-be96-91263232cc89@test.com"
}
```

```json
{
  "UserPartition": "3",
  "UserEmail": "7b1db853-a7cf-4889-be96-91263232cc89@test.com"
}
```

* Provide the partition, if known, to avoid cross-partition querying.

### Users Get Example

HTTP GET, POST:

```text
https://contact-service-1208.azurewebsites.net/api/UsersGet
```

Body:

```json
{
  "UserPartition": "c",
  "Skip": 0,
  "Take": 100
}
```

## GitHub Workflows

There are [workflows](.github/workflows) for:

| Workflow                          | Status
|-----------------------------------|---------------------------------------------------------------------------------------------------------
| Continuous Integration            | ![CI](https://github.com/Christoba/ContactService/workflows/CIWorkflow/badge.svg)
| Creating Azure Resources (manual) | ![AzureCreateResources](https://github.com/Christoba/ContactService/workflows/CreateResources/badge.svg)
| Delete Azure Resources (manual)   | ![AzureDeleteResources](https://github.com/Christoba/ContactService/workflows/DeleteResources/badge.svg)

All workflows run on the `ubuntu-latest` runner and can be seen [here](https://github.com/Christoba/ContactService/actions).

## Deploy Azure Resources

The [CreateAzureResources.ps1](automation/scripts/CreateAzureResources.ps1) script will create a resource group with all azure resources. The script requires [Azure Powershell 4.8.0](https://www.powershellgallery.com/packages/Az/4.8.0)

```powershell
.\automation\scripts\CreateAzureResources.ps1 -ClientSecret <secret> -Verbose
```

The resource group can be destroyed with [DeleteAzureResources.ps1](automation/scripts/DeleteAzureResources.ps1)

## Build

This repo employs [paket](https://fsprojects.github.io/Paket/index.html) package management and the dotnet cli for building.

Install [.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

```powershell
dotnet tool restore
dotnet paket install
dotnet build ./src/ContactService.sln
```

To update to the latest versions of packages and update the paket.lock

```powershell
dotnet tool restore
dotnet paket update
dotnet build ./src/ContactService.sln
```

## Test

NUnit tests are provided. After building, tests can be run via

```powershell
dotnet test ./src/ContactService.sln
```

## Publish

Publishing is done as part of CI/CD. You can manually publish as follows.

```powershell
dotnet build ./src/ContactService.sln -c Release
dotnet publish ./src/ContactService.sln -c Release
Compress-Archive -Path ".\src\ContactService\bin\Release\netstandard2.1\publish\*" -DestinationPath 'deployment.zip' -Force
.\automation\scripts\PublishAppService.ps1 -ClientSecret <secret> -DeployArchive 'deployment.zip' -Verbose
```

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md)