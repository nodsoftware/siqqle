# Use provided source directory or resolve from script location
$src = $env:BUILD_SOURCESDIRECTORY
if (-not $src)
{
    $src = Split-Path -Parent $PSCommandPath
    $src = Resolve-Path "$src/../"
}

# Always parse version from Directory.Build.props to ensure we have the latest version
Write-Host "Parsing version from Directory.Build.props..."
Write-Host "Source directory: $src"

$buildPropsPath = Join-Path $src 'Directory.Build.props'
Write-Host "Reading from: $buildPropsPath"

if (-not (Test-Path $buildPropsPath))
{
    Write-Error "Directory.Build.props not found at $buildPropsPath"
    exit 1
}

try
{
    [xml]$xml = Get-Content $buildPropsPath
    
    # Find the PropertyGroup element that contains SemanticVersion
    $propertyGroupWithVersion = $null
    foreach ($pg in $xml.Project.PropertyGroup)
    {
        if ($pg.SemanticVersion)
        {
            $propertyGroupWithVersion = $pg
            break
        }
    }
    
    if (-not $propertyGroupWithVersion)
    {
        Write-Error "PropertyGroup with SemanticVersion not found in Directory.Build.props"
        exit 1
    }
    
    $semanticVersion = $propertyGroupWithVersion.SemanticVersion.Trim()
    $prereleaseLabel = $propertyGroupWithVersion.PreReleaseLabel.Trim()
    
    if (-not $semanticVersion)
    {
        Write-Error "SemanticVersion not found in Directory.Build.props"
        exit 1
    }
    
    $versionParts = $semanticVersion.Split('.')
    if ($versionParts.Count -ne 3)
    {
        Write-Error "Invalid semantic version format: $semanticVersion (expected X.Y.Z)"
        exit 1
    }
    
    $env:MAJORVERSION = $versionParts[0]
    $env:MINORVERSION = $versionParts[1]
    $env:PATCHVERSION = $versionParts[2]
    
    if ($prereleaseLabel)
    {
        $env:PRERELEASELABEL = $prereleaseLabel
    }
    else
    {
        $env:PRERELEASELABEL = ""
    }
    
    Write-Host "Parsed version from Directory.Build.props: $semanticVersion"
    Write-Host "Parsed MAJORVERSION: $($env:MAJORVERSION), MINORVERSION: $($env:MINORVERSION), PATCHVERSION: $($env:PATCHVERSION)"
    if ($env:PRERELEASELABEL)
    {
        Write-Host "Parsed PreReleaseLabel: $env:PRERELEASELABEL"
    }
}
catch
{
    Write-Error "Failed to parse Directory.Build.props: $_"
    exit 1
}

# The source directory was already resolved above

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
    
    # Use LastWriteTime for better cross-platform compatibility
    # (CreationTime is unreliable on Linux)
    $fileInfo = Get-Item -Path $path
    $utcThen = $fileInfo.LastWriteTimeUtc

    $build = (((Get-YearsSince $utcNow $utcThen) % 7) * 10000) + [System.Int32]::Parse($utcNow.ToString("MMdd"))
    $revision = [System.Int32]::Parse($utcNow.ToString("HHmm"))

    return [System.Version]::new($major, $minor, $build, $revision)
}

# Get current commit hash.
$commitId = & git rev-parse --short HEAD 2>$null
if ($LASTEXITCODE -ne 0)
{
    Write-Warning "Failed to get git commit hash, using 'unknown'"
    $commitId = "unknown"
}

$majorVersion = $env:MAJORVERSION
$minorVersion = $env:MINORVERSION
$patchVersion = $env:PATCHVERSION
$prereleaseLabel = $env:PRERELEASELABEL

# Auto-generate version numbers using Directory.Build.props as reference file
$refFile = Join-Path $src 'Directory.Build.props'
$generatedVersion = Get-FileVersion $majorVersion $minorVersion $refFile

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
