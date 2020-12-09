[Diagnostics.CodeAnalysis.SuppressMessageAttribute("PSAvoidUsingConvertToSecureStringWithPlainText", "")]

[CmdletBinding()]
param (
  [Parameter(Mandatory)]
  [string]$ClientSecret
)

Import-Module 'Az' -RequiredVersion 4.8.0

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$configRoot = "$root\config"
$configuration = Get-Content "$configRoot\azure.json" | ConvertFrom-Json
$endpoints = Get-Content "$configRoot\endpoints.json" | ConvertFrom-Json

[securestring]$securePassword = ConvertTo-SecureString $ClientSecret -AsPlainText -Force
[pscredential]$credential = New-Object System.Management.Automation.PSCredential ($configuration.ClientId, $securePassword)

Write-Verbose "[$((Get-Date).ToUniversalTime())] Connecting to Azure"
$null = Connect-AzAccount -ServicePrincipal -Credential $credential -Tenant $configuration.TenantId -Subscription $configuration.SubscriptionId

# Create Resource Group
$rg = Get-AzResourceGroup -Name $configuration.ResourceGroupName -Location $configuration.PrimaryLocation -ErrorAction SilentlyContinue
if ($rg) {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Resource group '$($configuration.ResourceGroupName)' exists."
}
else {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Creating resource group '$($configuration.ResourceGroupName)'."
  $null = New-AzResourceGroup -Name $configuration.ResourceGroupName -Location $configuration.PrimaryLocation
}

# Create Cosmos DB Account
$locations = @(
  @{"locationName" = $configuration.PrimaryLocation; "failoverPriority" = 0 }
)
$iprangefilter = ""
$consistencyPolicy = @{"defaultConsistencyLevel" = "Session"; "maxIntervalInSeconds" = 5; "maxStalenessPrefix" = 100 }
$CosmosDBProperties = @{"databaseAccountOfferType" = "Standard"; "locations" = $locations; "consistencyPolicy" = $consistencyPolicy; "ipRangeFilter" = $iprangefilter }
$CosmosAccountResourceType = "Microsoft.DocumentDb/databaseAccounts"
$CosmosApiVersion = "2016-03-31"
$null = Register-AzResourceProvider -ProviderNamespace Microsoft.DocumentDb
$cdba = Get-AzResource -Name $configuration.CosmosDbAccountName -ResourceGroup $configuration.ResourceGroupName -ErrorAction Ignore
if ($cdba) {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Cosmos DB account '$($configuration.CosmosDbAccountName)' exists."
}
else {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Creating Cosmos DB account '$($configuration.CosmosDbAccountName)' in resource group '$($configuration.ResourceGroupName)'."
  $null = New-AzResource -ResourceType $CosmosAccountResourceType -ApiVersion $CosmosApiVersion -ResourceGroupName $configuration.ResourceGroupName -Location $configuration.PrimaryLocation -Name $configuration.CosmosDbAccountName -Properties $CosmosDBProperties -Force
}

# Create Cosmos Database
$databasesResourceName = $configuration.CosmosDbAccountName + "/sql/"
$dbs = Get-AzResource -ResourceType "Microsoft.DocumentDb/databaseAccounts/apis/databases" -ApiVersion $CosmosApiVersion -ResourceGroupName $configuration.ResourceGroupName -Name $databasesResourceName  | Select-Object Properties

if ($dbs.Properties.Id -eq $configuration.CosmosDbDatabaseName) {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Cosmos Database with name '$($configuration.CosmosDbDatabaseName) exists."
}
else {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Creating Cosmos Database with name '$($configuration.CosmosDbDatabaseName) in resource group '$($configuration.ResourceGroupName)' database account '$($configuration.CosmosDbAccountName)'."
  $newDatabaseResourceName = $configuration.CosmosDbAccountName + "/sql/" + $configuration.CosmosDbDatabaseName

  $DataBaseProperties = @{
    "resource" = @{"id" = $configuration.CosmosDbDatabaseName }
  }

  $null = New-AzResource -ResourceType "Microsoft.DocumentDb/databaseAccounts/apis/databases" -ApiVersion $CosmosApiVersion -ResourceGroupName $configuration.ResourceGroupName -Name $newDatabaseResourceName -PropertyObject $DataBaseProperties -Force
}

# Create Cosmos Container for Users
$containerResourceName = $configuration.CosmosDbAccountName + "/sql/" + $configuration.CosmosDbDatabaseName + "/" + $configuration.CosmosDbCollectionNameUser
$container = Get-AzResource -ResourceType "Microsoft.DocumentDb/databaseAccounts/apis/databases/containers" -ApiVersion $CosmosApiVersion -ResourceGroupName $configuration.ResourceGroupName -Name $containerResourceName -ErrorAction SilentlyContinue

if ($container) {
  Write-Verbose "Cosmos Container with name '$($configuration.CosmosDbCollectionNameUser) exists."
}
else {
  Write-Verbose "Creating Cosmos Container with name '$($configuration.CosmosDbCollectionNameUser) in resource group '$($configuration.ResourceGroupName)' database account '$($configuration.CosmosDbAccountName)' database '$($configuration.CosmosDbDatabaseName)'."

  $ContainerProperties = @{
    "resource" = @{
      "id"              = $configuration.CosmosDbCollectionNameUser;
      "partitionKey"    = @{
        "paths" = @("/UserPartition");
        "kind"  = "Hash"
      }
      "uniqueKeyPolicy" = @{
        "uniqueKeys" = @(
          @{ "paths" = @( "/UserEmail" ) }
        )
      }
    };
    "options"  = @{ "Throughput" = "400" }
  }

  $null = New-AzResource -ResourceType "Microsoft.DocumentDb/databaseAccounts/apis/databases/containers" -ApiVersion $CosmosApiVersion -ResourceGroupName $configuration.ResourceGroupName -Name $containerResourceName -PropertyObject $ContainerProperties -Force
}

# Create Cosmos Container for Contacts
$containerResourceName = $configuration.CosmosDbAccountName + "/sql/" + $configuration.CosmosDbDatabaseName + "/" + $configuration.CosmosDbCollectionNameContact
$container = Get-AzResource -ResourceType "Microsoft.DocumentDb/databaseAccounts/apis/databases/containers" -ApiVersion $CosmosApiVersion -ResourceGroupName $configuration.ResourceGroupName -Name $containerResourceName -ErrorAction SilentlyContinue

if ($container) {
  Write-Verbose "Cosmos Container with name '$($configuration.CosmosDbCollectionNameContact) exists."
}
else {
  Write-Verbose "Creating Cosmos Container with name '$($configuration.CosmosDbCollectionNameContact) in resource group '$($configuration.ResourceGroupName)' database account '$($configuration.CosmosDbAccountName)' database '$($configuration.CosmosDbDatabaseName)'."

  $ContainerProperties = @{
    "resource" = @{
      "id"              = $configuration.CosmosDbCollectionNameContact;
      "partitionKey"    = @{
        "paths" = @("/UserId");
        "kind"  = "Hash"
      }
      "uniqueKeyPolicy" = @{
        "uniqueKeys" = @(
          @{ "paths" = @( "/UserId", "/ContactEmail" ) }
        )
      }
    };
    "options"  = @{ "Throughput" = "400" }
  }

  $null = New-AzResource -ResourceType "Microsoft.DocumentDb/databaseAccounts/apis/databases/containers" -ApiVersion $CosmosApiVersion -ResourceGroupName $configuration.ResourceGroupName -Name $containerResourceName -PropertyObject $ContainerProperties -Force
}

# Get cosmos keys
Write-Verbose "Obtaining Cosmos DB account keys..."
$cosmosKeys = Invoke-AzResourceAction -Action listKeys -ResourceType $CosmosAccountResourceType -ApiVersion $CosmosApiVersion -ResourceGroupName $configuration.ResourceGroupName -Name $configuration.CosmosDbAccountName -Force
$cosmosConnectionString = "AccountEndpoint=https://$($configuration.CosmosDbAccountName).$($endpoints.Cosmos):443/;AccountKey=$($cosmosKeys.primaryMasterKey);"

# Create Storage Account
$sa = Get-AzStorageAccount -ResourceGroupName $configuration.ResourceGroupName -AccountName $configuration.StorageAccountName -ErrorAction SilentlyContinue
if ($sa) {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Storage account '$($configuration.StorageAccountName)' exists."
}
else {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Creating storage account '$($configuration.StorageAccountName)' in resource group '$($configuration.ResourceGroupName)'."
  $sa = New-AzStorageAccount -ResourceGroupName $configuration.ResourceGroupName -AccountName $configuration.StorageAccountName -Location $configuration.PrimaryLocation -SkuName "Standard_LRS"
}

Write-Verbose "[$((Get-Date).ToUniversalTime())] Obtaining storage account keys..."
$storageAccountKeys = Get-AzStorageAccountKey -ResourceGroupName $configuration.ResourceGroupName -AccountName $configuration.StorageAccountName
$accountKey = $storageAccountKeys | Where-Object { $_.KeyName -eq "Key1" } | Select-Object Value
$storageAccountConnectionString = "DefaultEndpointsProtocol=https;AccountName=$($configuration.StorageAccountName);AccountKey=$($accountKey.Value);EndpointSuffix=$($endpoints.Storage)"

# Create Application Insight
$appInsights = Get-AzApplicationInsights -ResourceGroupName $configuration.ResourceGroupName -Name $configuration.ApplicationInsights -ErrorAction SilentlyContinue
if ($appInsights) {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] App insights '$($configuration.ApplicationInsights)' exists."
}
else {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Creating app insights '$($configuration.ApplicationInsights)' in resource group '$($configuration.ResourceGroupName)'."
  $appInsights = New-AzApplicationInsights -ResourceGroupName $configuration.ResourceGroupName -Name $configuration.ApplicationInsights -location $configuration.PrimaryLocation
}

# Create App Services
$FunctionAppResourceType = "Microsoft.Web/Sites"
$null = Register-AzResourceProvider -ProviderNamespace Microsoft.Web

$fa = Get-AzResource -Name $configuration.FunctionAppName -ResourceGroup $configuration.ResourceGroupName  -ResourceType $FunctionAppResourceType -ErrorAction SilentlyContinue
if ($fa) {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Function app $($configuration.FunctionAppName) exists."
}
else {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Creating function app $($configuration.FunctionAppName) in resource group '$($configuration.ResourceGroupName)'."
  $fa = New-AzResource -ResourceType $FunctionAppResourceType -kind 'functionapp' -ResourceGroupName $configuration.ResourceGroupName -Location $configuration.PrimaryLocation -Name $configuration.FunctionAppName -Properties @{ } -Force
}

# Update App Settings
Write-Verbose "[$((Get-Date).ToUniversalTime())] Updating app settings..."
$AppSettings = @{
  "APPINSIGHTS_INSTRUMENTATIONKEY"           = $appInsights.InstrumentationKey
  "AzureWebJobsDashboard"                    = $storageAccountConnectionString
  "AzureWebJobsStorage"                      = $storageAccountConnectionString
  "CosmosDbAccountName"                      = $configuration.CosmosDbAccountName
  "CosmosDbDatabaseName"                     = $configuration.CosmosDbDatabaseName
  "CosmosDbEndpoint"                         = $endpoints.Cosmos
  "CosmosDbConnectionString"                 = $cosmosConnectionString
  "CosmosDbCollectionNameUser"               = $configuration.CosmosDbCollectionNameUser
  "CosmosDbCollectionNameContact"            = $configuration.CosmosDbCollectionNameContact
  "CosmosAuthKey"                            = $cosmosKeys.primaryMasterKey
  "FUNCTIONS_EXTENSION_VERSION"              = "~3"
  "FUNCTIONS_WORKER_RUNTIME"                 = "dotnet"
  "WEBSITE_CONTENTAZUREFILECONNECTIONSTRING" = $storageAccountConnectionString
  "WEBSITE_CONTENTSHARE"                     = $configuration.FunctionAppName
  "WEBSITE_RUN_FROM_PACKAGE"                 = "1"
}
$fa = Set-AzWebApp -Name $configuration.FunctionAppName -ResourceGroupName $configuration.ResourceGroupName -AppSettings $AppSettings -AssignIdentity $true

Write-Verbose "[$((Get-Date).ToUniversalTime())] Resource creation complete."