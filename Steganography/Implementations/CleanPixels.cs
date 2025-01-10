using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Consts;
using SteganographyInPicture.DTO;
using SteganographyInPicture.Steganography.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyInPicture.Steganography.Implementations;

class CleanPixels : ICleanPixels
{
    Random Random = new Random(DateTime.Now.ToString().GetHashCode());

    public Image CleanImage(Image image, int depth)
    {
        // Копия изображения.
        var cloneImage = image.CloneAs<Rgb24>();

        // Массив пикселей.
        var arrayOfPixels = new Rgb24[cloneImage.Width * cloneImage.Height];

        // Копирование пикселей изображения в массив пикселей.
        cloneImage.CopyPixelDataTo(arrayOfPixels);

        var zeroingMask = ~((1 << depth) - 1);

        var randomValue = default(int);

        for (int i = 0; i < arrayOfPixels.Length; i++)
        {
            for (int chanel = 0; chanel < EncodingConsts.CHANNEL_COUNT; chanel++)
            {
                randomValue = GenerateNumberWithBits(depth);
                _ = chanel switch
                {
                    0 => arrayOfPixels[i].R = (byte)(arrayOfPixels[i].R & zeroingMask | randomValue),
                    1 => arrayOfPixels[i].G = (byte)(arrayOfPixels[i].G & zeroingMask | randomValue),
                    2 => arrayOfPixels[i].B = (byte)(arrayOfPixels[i].B & zeroingMask | randomValue),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        // Изображение с затертой информацией.
        return Image.LoadPixelData(new ReadOnlySpan<Rgb24>(arrayOfPixels), cloneImage.Width, cloneImage.Height);
    }

    private int GenerateNumberWithBits(int depth)
    {
        int result = 0;

        while(depth > 0)
        {
            result <<= 1;
            result |= Random.Next(2);
            depth--;
        }

        return result;
    }

}
