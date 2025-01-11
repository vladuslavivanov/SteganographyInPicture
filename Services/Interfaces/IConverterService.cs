using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteganographyInPicture.Services.Interfaces;

public interface IConverterService
{
    Task<List<Rgb24>> ConvertTextToRgb24ArrayAsync(string text, int availableBitsInChannel, EncodingEnum encoding, CompressionsEnum compression);

    bool CheckArrayOfBytesOnNullTermenator(List<byte> bytes, EncodingEnum encoding);
}
