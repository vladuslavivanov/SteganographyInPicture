using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteganographyInPicture.Services.Interfaces;

public interface IConverterService
{
    Task<List<Rgb24>> ConvertTextToRgb24ArrayAsync(string text, IEnumerable<PixelChannelModel> PixelsBitDepth, EncodingEnum encoding, CompressionsEnum compression);
}
