using Microsoft.UI.Xaml.Media.Imaging;
using SixLabors.ImageSharp;
using SteganographyInPicture.Enums;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SteganographyInPicture.Extensions;

internal static class ImageExtension
{
    public static async Task<BitmapImage> SaveAsImageAsync(this Image image, ImageExtensionsEnum imageType)
    {
        using (var memoryStream = new MemoryStream())
        {
            switch (imageType)
            {
                case ImageExtensionsEnum.bmp:
                    await image.SaveAsBmpAsync(memoryStream);
                    break;
                case ImageExtensionsEnum.gif:
                    await image.SaveAsGifAsync(memoryStream);
                    break;
                case ImageExtensionsEnum.jpeg:
                    await image.SaveAsJpegAsync(memoryStream);
                    break;
                case ImageExtensionsEnum.png:
                    await image.SaveAsPngAsync(memoryStream);
                    break;
                case ImageExtensionsEnum.tiff:
                    await image.SaveAsTiffAsync(memoryStream);
                    break;
                default:
                    break;
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            var result = new BitmapImage();
            result.SetSource(memoryStream.AsRandomAccessStream());
            return result;
        }
    }
}
