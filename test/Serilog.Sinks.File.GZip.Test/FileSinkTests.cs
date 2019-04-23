using System.IO;
using Serilog.Sinks.File.GZip.Tests.Support;
using Shouldly;
using Xunit;

namespace Serilog.Sinks.File.GZip.Tests
{
    public class FileSinkTests
    {
        [Fact]
        public void Should_compress_log_file()
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

                using (var log = new LoggerConfiguration()
                    .WriteTo.File(Path.Combine(path), hooks: gzipWrapper)
                    .CreateLogger())
                {
                    foreach (var logEvent in logEvents)
                    {
                        log.Write(logEvent);
                    }
                }

                // Ensure the data was written through the wrapping GZipStream, by decompressing and comparing against
                // what we wrote
                var lines = Utils.DecompressLines(path);
                
                lines.Count.ShouldBe(logEvents.Length);
                
                for (var i = 0; i < logEvents.Length; i++)
                {
                    lines[i].ShouldEndWith(logEvents[i].MessageTemplate.Text);
                }
            }
        }
    }
}
