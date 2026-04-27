# OruTeaDa (ChaiIdle) — Release Packager
# Usage: .\release.ps1 -Version "1.0.0"

param(
    [Parameter(Mandatory=$true)]
    [string]$Version
)

$ProjectName  = "ChaiIdle"
$AppName      = "OruTeaDa"
$ProjectRoot  = Split-Path -Parent $MyInvocation.MyCommand.Path
$PublishDir   = "$ProjectRoot\bin\Release\net8.0-windows\win-x64\publish"
$ReleaseDir   = "$ProjectRoot\release"
$ZipName      = "$AppName-v$Version-win-x64.zip"
$ZipPath      = "$ReleaseDir\$ZipName"

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  OruTeaDa — Release Packager v$Version" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# ── 1. Clean previous release ──────────────────────────────────────────────
if (Test-Path $ReleaseDir) { Remove-Item $ReleaseDir -Recurse -Force }
New-Item -ItemType Directory -Path $ReleaseDir | Out-Null
Write-Host "[1/4] Release directory ready." -ForegroundColor Yellow

# ── 2. Publish single-file EXE ────────────────────────────────────────────
Write-Host "[2/4] Publishing single-file EXE (this may take 2-3 min)..." -ForegroundColor Yellow
Set-Location $ProjectRoot

dotnet publish -c Release -r win-x64 `
    --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    /p:DebugType=embedded `
    /p:PublishTrimmed=false `
    /p:Version=$Version | Out-Null

if ($LASTEXITCODE -ne 0) {
    Write-Host "BUILD FAILED. Fix errors and retry." -ForegroundColor Red
    exit 1
}
Write-Host "  EXE built successfully." -ForegroundColor Green

# ── 3. Copy EXE + Assets into a staging folder ────────────────────────────
Write-Host "[3/4] Packaging EXE + Assets..." -ForegroundColor Yellow

$StagingDir = "$ReleaseDir\$AppName-v$Version"
New-Item -ItemType Directory -Path $StagingDir | Out-Null

# Copy the EXE
Copy-Item "$PublishDir\$ProjectName.exe" "$StagingDir\$AppName.exe"

# Copy Assets folder (audio, icons, etc.)
if (Test-Path "$ProjectRoot\Assets") {
    Copy-Item "$ProjectRoot\Assets" "$StagingDir\Assets" -Recurse
}

# Write a quick README
@"
OruTeaDa — ChaiIdle v$Version
================================
A chai break reminder for developers.

USAGE
-----
Double-click OruTeaDa.exe to start.
It will appear in the system tray.
After your set idle time, a chai break popup appears.

AUDIO (optional)
----------------
Place language-specific MP3s in:
  Assets\Audio\Tamil\    ← Tamil dialogues
  Assets\Audio\English\  ← English dialogues
  Assets\Audio\Hinglish\ ← Hinglish dialogues

The existing Assets\tea_dialogue.mp3 works as a fallback.

REQUIREMENTS
------------
Windows 10/11 (64-bit). No installation needed.
"@ | Set-Content "$StagingDir\README.txt"

Write-Host "  Staged: $StagingDir" -ForegroundColor Green

# ── 4. Zip the staging folder ─────────────────────────────────────────────
Write-Host "[4/4] Creating ZIP archive..." -ForegroundColor Yellow
Compress-Archive -Path "$StagingDir\*" -DestinationPath $ZipPath -Force
Write-Host "  ZIP ready: $ZipPath" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  RELEASE PACK READY" -ForegroundColor Green
Write-Host "  $ZipPath" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "NEXT STEPS:" -ForegroundColor Cyan
Write-Host "  1. git tag v$Version && git push origin v$Version" -ForegroundColor White
Write-Host "  2. GitHub Actions will auto-build & publish the release" -ForegroundColor White
Write-Host "  3. OR upload $ZipName manually via GitHub > Releases > Draft" -ForegroundColor White
Write-Host ""
