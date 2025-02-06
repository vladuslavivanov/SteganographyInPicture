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

    public string DecryptPhoto(DecryptPhotoDto decryptPhotoDto) =>
        ImageSteganography.DecryptPhoto(decryptPhotoDto, PixelSelector);

    public async Task<EncryptPhotoResultDto> EncryptPhotoAsync(EncryptPhotoDto encryptPhotoDto) =>
        await ImageSteganography.EncryptPhotoAsync(encryptPhotoDto, PixelSelector);

    IEnumerable<int> PixelSelector(PixelSelectorDto pixelSelectorDto)
    {
        #region Валидация

        if (pixelSelectorDto.FrequencyOfGroups < 1)
        {
            throw new ArgumentOutOfRangeException("Частота групп не может быть менее 1.");
        }

        if (pixelSelectorDto.QuantityPixelsInGroup < 1)
        {
            throw new ArgumentOutOfRangeException("Количество пикселей в группе не может быть менее 1.");
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
