using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Enums;
using System.Collections.Generic;

namespace SteganographyInPicture.Services.Interfaces;

public interface IConverterService
{
    List<Rgb24> ConvertTextToRgb24Array(string text, int availableBitsInChannel, EncodingEnum encoding);

    bool CheckArrayOfBytesOnNullTermenator(List<byte> bytes, EncodingEnum encoding);
}
