# Serilog.Sinks.File.GZip
[![NuGet](https://img.shields.io/nuget/v/Serilog.Sinks.File.GZip.svg)](https://www.nuget.org/packages/Serilog.Sinks.File.GZip)
[![Build status](https://ci.appveyor.com/api/projects/status/88es7b73dbc47b9s?svg=true)](https://ci.appveyor.com/project/cocowalla/serilog-sinks-file-gzip)

A `FileLifecycleHooks`-based plugin for the [Serilog File Sink](https://github.com/serilog/serilog-sinks-file) that compresses log files using streaming GZip compression.

### Getting started
To get started, install the latest [Serilog.Sinks.File.GZip](https://www.nuget.org/packages/Serilog.Sinks.File.GZip) package from NuGet:

```powershell
Install-Package Serilog.Sinks.File.GZip -Version 1.0.1
```

To enable GZip compression, use one of the new `LoggerSinkConfiguration` extensions that has a `FileLifecycleHooks` argument, and create a new `GZipHooks`:

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.gz", hooks: new GZipHooks())
    .CreateLogger();
```

Note this also works if you enable rolling log files.

You can optionally tweak GZIP compression by changing the log level and buffer size (the default is 32KB):

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.gz", hooks: new GZipHooks(CompressionLevel.Fastest, 1024 * 64))
    .CreateLogger();
```

It's also possible to enable GZIP compression when configuring Serilog from a configuration file using [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration/):

```json
{
  "Serilog": {
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "log.gz",
          "hooks": "Serilog.Sinks.File.GZip.GZipHooks, Serilog.Sinks.File.GZip"
        }
      }
    ]
  }
}
```

Larger buffer sizes potentially result in better compression ratios, but note that in the event of a crash that the contents of the buffer may be lost.

As is [standard with Serilog](https://github.com/serilog/serilog/wiki/Lifecycle-of-Loggers#in-all-apps), it's important to call `Log.CloseAndFlush();` before your application ends.

### About `FileLifecycleHooks`
`FileLifecycleHooks` is a Serilog File Sink mechanism that allows hooking into log file lifecycle events, enabling scenarios such as wrapping the Serilog output stream in another stream, or capturing files before they are deleted by Serilog's retention mechanism.

Other available hooks include:

- [serilog-sinks-file-header](https://github.com/cocowalla/serilog-sinks-file-header): writes a header to the start of each log file
- [serilog-sinks-file-archive](https://github.com/cocowalla/serilog-sinks-file-archive): archives completed log files before they are deleted by Serilog's retention mechanism
