# OruTeaDa Release Packager
param(
    [Parameter(Mandatory=$true)]
    [string]$Version
)

$ProjectName  = "ChaiIdle"
$AppName      = "OruTeaDa"
$ProjectRoot  = Get-Location
$ReleaseDir   = Join-Path $ProjectRoot "release"
$StagingDir   = Join-Path $ReleaseDir "$AppName-v$Version"
$ZipPath      = Join-Path $ReleaseDir "$AppName-v$Version-win-x64.zip"

Write-Host "--- OruTeaDa Release Packager v$Version ---"

# 1. Clean and Create Dirs
if (Test-Path $ReleaseDir) { Remove-Item $ReleaseDir -Recurse -Force }
New-Item -ItemType Directory -Path $ReleaseDir | Out-Null
New-Item -ItemType Directory -Path $StagingDir | Out-Null

# 2. Build
Write-Host "Step 2: Publishing EXE. Please wait..."
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true /p:DebugType=embedded /p:PublishTrimmed=false /p:Version=$Version

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!"
    exit 1
}

# 3. Copy files
Write-Host "Step 3: Packaging files..."
$PublishDir = Join-Path $ProjectRoot "bin\Release\net8.0-windows\win-x64\publish"
Copy-Item (Join-Path $PublishDir "$ProjectName.exe") (Join-Path $StagingDir "$AppName.exe")

if (Test-Path (Join-Path $ProjectRoot "Assets")) {
    Copy-Item (Join-Path $ProjectRoot "Assets") $StagingDir -Recurse
}

# 4. Create ZIP
Write-Host "Step 4: Creating ZIP..."
Compress-Archive -Path "$StagingDir\*" -DestinationPath $ZipPath -Force

Write-Host "Done! ZIP created at: $ZipPath"
