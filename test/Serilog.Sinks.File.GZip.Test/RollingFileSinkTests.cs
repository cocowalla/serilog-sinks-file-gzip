using System;
using System.IO;
using System.Linq;
using Serilog.Sinks.File.GZip.Tests.Support;
using Shouldly;
using Xunit;

namespace Serilog.Sinks.File.GZip.Tests
{
    public class RollingFileSinkTests
    {
        [Fact]
        public void Should_compress_rolling_log_files()
        {
            var gzipWrapper = new GZipHooks();

            var logEvents = new[]
            {
                Some.LogEvent(),
                Some.LogEvent(),
                Some.LogEvent()
            };
            
            using (var temp = TempFolder.ForCaller())
            {
                var path = temp.AllocateFilename("gz");

                // Use a rolling log file configuration with a 1-byte size limit, so we roll after each log event
                using (var log = new LoggerConfiguration()
                    .WriteTo.File(path, rollOnFileSizeLimit: true, fileSizeLimitBytes: 1, hooks: gzipWrapper)
                    .CreateLogger())
                {
                    foreach (var logEvent in logEvents)
                    {
                        log.Write(logEvent);
                    }
                }
                
                // Get all the files the rolling file sink wrote
                var files = Directory.GetFiles(temp.Path)
                    .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                    .ToArray();
                
                // We should have a file for each entry in logEvents
                files.Length.ShouldBe(logEvents.Length);

                // Ensure the data was written through the wrapping GZipStream, by decompressing and comparing against
                // what we wrote
                for (var i = 0; i < files.Length; i++)
                {
                    var lines = Utils.DecompressLines(files[i]);
                    
                    lines.Count.ShouldBe(1);
                    lines[0].ShouldEndWith(logEvents[i].MessageTemplate.Text);
                }
            }
        }
    }
}
