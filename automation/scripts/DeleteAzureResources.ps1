[cmdletbinding()]
param (
  [Parameter(Mandatory)]
  [string]$ClientSecret
)

$root = Split-Path -Parent $PSScriptRoot
$configRoot = "$root\config"

$configuration = Get-Content "$configRoot\azure.json" | ConvertFrom-json

[securestring]$securePassword = ConvertTo-SecureString $ClientSecret -AsPlainText -Force
[pscredential]$credential = New-Object System.Management.Automation.PSCredential ($configuration.ClientId, $securePassword)

Write-Verbose "[$((Get-Date).ToUniversalTime())] Connecting to Azure"
$null = Connect-AzAccount -ServicePrincipal -Credential $credential -Tenant $configuration.TenantId -Subscription $configuration.SubscriptionId

# Delete Resource Group
$rg = Get-AzResourceGroup -Name $configuration.ResourceGroupName -Location $configuration.PrimaryLocation -ErrorAction Ignore
if ($rg) {
  # Unlock Resources
  if ($UnlockResources) {
    Write-Verbose "[$((Get-Date).ToUniversalTime())] Unlocking resources for group '$($configuration.ResourceGroupName)'."
    $null = Get-AzResourceLock -ResourceGroupName $configuration.ResourceGroupName | ReMove-AzResourceLock -Force
  }

  Write-Verbose "[$((Get-Date).ToUniversalTime())] Resource group '$($configuration.ResourceGroupName)' exists and will be deleted."
  $null = Remove-AzResourceGroup -Name $configuration.ResourceGroupName -Force
}
else {
  Write-Verbose "[$((Get-Date).ToUniversalTime())] Resource group '$($configuration.ResourceGroupName)' does not exist."
}

Write-Verbose "[$((Get-Date).ToUniversalTime())] Resource deletion complete."