#!/bin/bash
# Da, Linux/Mac build script ba! 🍵

set -e

echo "===================================="
echo "ChaiIdle - Build Script (Linux/Mac)"
echo "Da, oru tea vadikka sollrindhu thiyanum ba!"
echo "===================================="
echo ""

cd "$(dirname "$0")"

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found!"
    echo "Install from: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

echo "✅ .NET SDK found"
echo ""

# Display menu
echo "Choose build type:"
echo "1 - Debug (fast, for testing)"
echo "2 - Release (optimized)"
echo "3 - Single-File EXE (for Windows viral launch!)"
echo ""

read -p "Enter choice [1-3]: " choice

case $choice in
    1)
        echo ""
        echo "Building Debug version..."
        dotnet build -c Debug
        echo ""
        echo "✅ Debug build complete!"
        echo "Run: dotnet run"
        ;;
    2)
        echo ""
        echo "Building Release version..."
        dotnet build -c Release
        echo ""
        echo "✅ Release build complete!"
        echo "Run: bin/Release/net8.0/ChaiIdle"
        ;;
    3)
        echo ""
        echo "🚀 Publishing single-file Windows executable..."
        echo "This may take 2-3 minutes..."
        echo ""
        dotnet publish -c Release -r win-x64 \
            --self-contained true \
            /p:PublishSingleFile=true \
            /p:IncludeNativeLibrariesForSelfExtract=true \
            /p:DebugType=embedded \
            /p:PublishTrimmed=false
        echo ""
        echo "✅ Single-file EXE created!"
        echo "Location: bin/Release/net8.0-windows/win-x64/publish/ChaiIdle.exe"
        echo ""
        ;;
    *)
        echo "❌ Invalid choice!"
        exit 1
        ;;
esac

echo ""
