dotnet restore .\serilog-sinks-file-gzip.sln
dotnet build .\src\Serilog.Sinks.File.GZip\Serilog.Sinks.File.GZip.csproj --configuration Release

dotnet test .\test\Serilog.Sinks.File.GZip.Test\Serilog.Sinks.File.GZip.Test.csproj

dotnet pack .\src\Serilog.Sinks.File.GZip\Serilog.Sinks.File.GZip.csproj -c Release
