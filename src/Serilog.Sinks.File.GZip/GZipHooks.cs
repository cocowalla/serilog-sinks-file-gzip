using System.IO;
using System.IO.Compression;
using System.Text;

namespace Serilog.Sinks.File.GZip
{
    /// <inheritdoc />
    /// <summary>
    /// Compresses log output using streaming GZip
    /// </summary>
    public class GZipHooks : FileLifecycleHooks
    {
        private const int DEFAULT_BUFFER_SIZE = 32 * 1024;
        
        private readonly CompressionLevel compressionLevel;
        private readonly int bufferSize;
        
        public GZipHooks(CompressionLevel compressionLevel = CompressionLevel.Fastest, int bufferSize = DEFAULT_BUFFER_SIZE)
        {
            this.compressionLevel = compressionLevel;
            this.bufferSize = bufferSize;
        }

        public override Stream OnFileOpened(Stream underlyingStream, Encoding _)
        {
            var compressStream = new GZipStream(underlyingStream, this.compressionLevel);
            return new BufferedStream(compressStream, this.bufferSize);
        }
    }
}
