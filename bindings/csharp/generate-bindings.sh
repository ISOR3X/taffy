#!/bin/sh

cargo build --release -p csharp-taffy
dotnet build bindings/csharp/include/Taffy.csproj --configuration Release
