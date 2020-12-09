[Diagnostics.CodeAnalysis.SuppressMessageAttribute("PSAvoidUsingConvertToSecureStringWithPlainText", "")]

[CmdletBinding()]
param (
  [Parameter(Mandatory)]
  [string]$ClientSecret,

  [Parameter()]
  [string]$DeployArchive = 'deployment.zip'
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

Write-Verbose "[$((Get-Date).ToUniversalTime())] Getting publishing profile..."
$publishProfile = Get-AzWebAppPublishingProfile -ResourceGroupName $configuration.ResourceGroupName -Name $configuration.FunctionAppName

$null = $publishProfile -match "userName=""(.+?)"""
$deployUser = $Matches[1]
$null = $publishProfile -match "userPWD=""(.+?)"""
$deployPassword = $Matches[1]
$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(("{0}:{1}" -f $deployUser, $deployPassword)))

$publishUri = "https://$($configuration.FunctionAppName).scm.$($endpoints.Function)/api/zipdeploy"
Write-Verbose "Publishing to '$publishUri' with archive '$DeployArchive' ..."
Invoke-RestMethod -Uri $publishUri -Headers @{Authorization = ("Basic {0}" -f $base64AuthInfo) } -UserAgent "powershell/1.0" -Method POST -InFile $DeployArchive -ContentType "multipart/form-data"
Write-Verbose "Publish complete."
