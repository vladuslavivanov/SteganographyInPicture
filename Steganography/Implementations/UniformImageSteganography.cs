using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Consts;
using SteganographyInPicture.DTO;
using SteganographyInPicture.Steganography.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteganographyInPicture.Steganography.Implementations;

internal class UniformImageSteganography : IImageSteganography
{
    #region Singleton

    private UniformImageSteganography() { }

    private static readonly Lazy<UniformImageSteganography> _instance =
        new(() => new());

    public static UniformImageSteganography Instance => _instance.Value;

    #endregion

    public string DecryptPhoto(DecryptPhotoDto decryptPhotoDto)
    {
        #region Валидация

        if (decryptPhotoDto.BitDepth < 1)
        {
            throw new ArgumentException(null, "Глубина встраивания не может быть менее 1.");
        }

        #endregion

        var result = ImageSteganography.DecryptPhoto(decryptPhotoDto, PixelSelector);
        
        return result.Remove(result.Length - 1);
    }

    public async Task<EncryptPhotoResultDto> EncryptPhotoAsync(EncryptPhotoDto encryptPhotoDto)
    {
        return await ImageSteganography.EncryptPhotoAsync(encryptPhotoDto, PixelSelector);
    }

    IEnumerable<int> PixelSelector(PixelSelectorDto pixelSelectorDto)
    {
        #region Валидация

        if (pixelSelectorDto.FrequencyOfGroups < 1)
        {
            throw new ArgumentOutOfRangeException(null, 
                "Частота групп не может быть менее 1.");
        }

        if (pixelSelectorDto.QuantityPixelsInGroup < 1)
        {
            throw new ArgumentOutOfRangeException(null, 
                "Количество пикселей в группе не может быть менее 1.");
        }

        #endregion

        var pixels = pixelSelectorDto.Pixels;

        // Цикл по пикселям изображения с шагом частоты групп.
        for (int i = 0; i < pixels.Length; i += pixelSelectorDto.FrequencyOfGroups)
        {
            // Цикл по количествам писелей в группе.
            for (int j = 0; j < pixelSelectorDto.QuantityPixelsInGroup; j++)
            {
                yield return i + j;
            }
        }
    }
}
