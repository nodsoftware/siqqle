if(-not $env:MAJORVERSION)
{
    Write-Host "You must set the following environment variables to test this script interactively:"
    Write-Host '$env:MAJORVERSION = 4'
    Write-Host '$env:MINORVERSION = 0'
    Write-Host '$env:PATCHVERSION = 1'
	Write-Host
	Write-Host "Optionally you can set a pre-release label too:"
    Write-Host '$env:PRERELEASELABEL = "alpha"'
	Write-Host
	Write-Error "Required environment variables not found."
    exit 1
}

function Get-ScriptDirectory 
{
	Split-Path -Parent $PSCommandPath
}

$src = $env:BUILD_SOURCESDIRECTORY
if (-not $src)
{
	$src = Get-ScriptDirectory
	$src = Resolve-Path "$src/../"
}

function Get-YearsSince($now, $then)
{
    $years = ($now.Year - $then.Year)
    if ($now.Month -lt $then.Month)
    {
        $years -= 1
    }

    return $years
}

function Get-FileVersion($major, $minor, $path)
{
    $utcNow = [System.DateTimeOffset]::UtcNow
    $utcThen = (Get-ItemPropertyValue -Name CreationTime -Path $path).ToUniversalTime()

    $build = (((Get-YearsSince $utcNow $utcThen) % 7) * 10000) + [System.Int32]::Parse($utcNow.ToString("MMdd"))
    $revision = [System.Int32]::Parse($utcNow.ToString("HHmm"))

    return [System.Version]::new($major, $minor, $build, $revision)
}

# Get current commit hash.
$commitId = git rev-parse --short HEAD
$majorVersion = $env:MAJORVERSION
$minorVersion = $env:MINORVERSION
$patchVersion = $env:PATCHVERSION
$prereleaseLabel = $env:PRERELEASELABEL

# Auto-generate version numbers.
$generatedVersion = Get-FileVersion $majorVersion $minorVersion (Join-Path $src 'src/Siqqle.sln')

$packageVersion = "$majorVersion.$minorVersion.$patchVersion"
if (-not $prereleaseLabel -eq "")
{
	$packageVersion = "$packageVersion-$prereleaseLabel.$($generatedVersion.Build).$($generatedVersion.Revision)"
}

$assemblyVersion = "$majorVersion.$minorVersion.$patchVersion"
$assemblyFileVersion = $generatedVersion.ToString()

$productVersion = "$assemblyVersion"
if (-not $prereleaseLabel -eq "")
{
	$productVersion = "$productVersion $prereleaseLabel"
}
$productVersion += " (rev. $commitId)"

$productName = "Siqqle $assemblyVersion"
if (-not $prereleaseLabel -eq "")
{
	$productName = "$productName $prereleaseLabel"
}

Write-Host "Package Version: $packageVersion"
Write-Host "Assembly Version: $assemblyVersion"
Write-Host "Assembly File Version: $assemblyFileVersion"
Write-Host "Product Name: $productName"
Write-Host "Product Version: $productVersion"

# Publish variables to VSTS.
Write-Host "##vso[task.setvariable variable=PackageVersion;]$packageVersion"
Write-Host "##vso[task.setvariable variable=AssemblyVersion;]$assemblyVersion"
Write-Host "##vso[task.setvariable variable=FileVersion;]$assemblyFileVersion"
Write-Host "##vso[task.setvariable variable=ProductName;]$productName"
Write-Host "##vso[task.setvariable variable=ProductVersion;]$productVersion"
Write-Host "##vso[task.setvariable variable=CommitId;]$commitId"
