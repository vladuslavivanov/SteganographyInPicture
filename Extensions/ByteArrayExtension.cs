using SteganographyInPicture.Enums;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace SteganographyInPicture.Extensions;

static class ByteArrayExtension
{
    public static async Task<byte[]> CompressBytesAsync(this byte[] array, CompressionsEnum compressionType)
    {
        switch (compressionType)
        {
            case CompressionsEnum.Gzip:
                using (var outputStream = new MemoryStream())
                {
                    using var gzipStream = new GZipStream(outputStream, CompressionLevel.SmallestSize);
                    await gzipStream.WriteAsync(array);
                    await gzipStream.FlushAsync();
                    return outputStream.ToArray();
                }
            case CompressionsEnum.Brotli:
                using (var outputStream = new MemoryStream())
                {
                    using var brotliStream = new BrotliStream(outputStream, CompressionLevel.SmallestSize);
                    await brotliStream.WriteAsync(array);
                    await brotliStream.FlushAsync();
                    return outputStream.ToArray();
                }                
            case CompressionsEnum.Deflate:
                using (var outputStream = new MemoryStream())
                {
                    using var deflateStream = new DeflateStream(outputStream, CompressionLevel.SmallestSize);
                    await deflateStream.WriteAsync(array);
                    await deflateStream.FlushAsync();
                    return outputStream.ToArray();
                }
            case CompressionsEnum.None:
                return array.ToArray();
            default:
                throw new NotImplementedException(nameof(compressionType));
        }
    }

    public static async Task<byte[]> DecompressBytesAsync(this byte[] array, CompressionsEnum compressionType)
    {
        switch (compressionType)
        {
            case CompressionsEnum.Gzip:
                using (var inputStream = new MemoryStream(array))
                {
                    using var outputStream = new MemoryStream();
                    using var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
                    try
                    {
                        await gzipStream.CopyToAsync(outputStream);
                    }
                    catch (Exception ex) { }
                    await gzipStream.FlushAsync();
                    return outputStream.ToArray();
                }
            case CompressionsEnum.Brotli:
                using (var inputStream = new MemoryStream(array))
                {
                    using var outputStream = new MemoryStream();
                    using var brotliStream = new BrotliStream(inputStream, CompressionMode.Decompress);
                    await brotliStream.CopyToAsync(outputStream);
                    await brotliStream.FlushAsync();
                    return outputStream.ToArray();
                }
            case CompressionsEnum.Deflate:
                using (var inputStream = new MemoryStream(array))
                {
                    using var outputStream = new MemoryStream();
                    using var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress);
                    await deflateStream.CopyToAsync(outputStream);
                    await deflateStream.FlushAsync();
                    return outputStream.ToArray();
                }
            case CompressionsEnum.None:
                return array.ToArray();
            default:
                throw new NotImplementedException(nameof(compressionType));
        }
    }

}
