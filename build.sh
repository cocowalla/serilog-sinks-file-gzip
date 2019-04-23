#!/bin/bash
set -ev

dotnet restore ./serilog-sinks-file-gzip.sln --runtime netstandard2.0
dotnet build ./src/Serilog.Sinks.File.GZip/Serilog.Sinks.File.GZip.csproj --runtime netstandard2.0 --configuration Release

dotnet test ./test/Serilog.Sinks.File.GZip.Test/Serilog.Sinks.File.GZip.Test.csproj --framework netcoreapp2.2
