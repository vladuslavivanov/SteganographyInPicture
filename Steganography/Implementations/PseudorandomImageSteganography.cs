using SixLabors.ImageSharp;
using SteganographyInPicture.DTO;
using SteganographyInPicture.Steganography.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteganographyInPicture.Steganography.Implementations;

internal class PseudorandomImageSteganography : IImageSteganography
{
    #region Singleton

    private PseudorandomImageSteganography() { }

    private static readonly Lazy<PseudorandomImageSteganography> _instance =
        new(() => new());

    public static PseudorandomImageSteganography Instance => _instance.Value;

    #endregion

    public string DecryptPhoto(DecryptPhotoDto decryptPhotoDto)
    {
        var result =
            ImageSteganography.DecryptPhoto(decryptPhotoDto, PixelSelector);

        return result.Remove(result.Length - 1);
    }

    public async Task<EncryptPhotoResultDto> EncryptPhotoAsync(EncryptPhotoDto encryptPhotoDto)
    {
        return await ImageSteganography.EncryptPhotoAsync(encryptPhotoDto, PixelSelector);
    }

    IEnumerable<int> PixelSelector(PixelSelectorDto pixelSelectorDto)
    {
        #region Валидация

        if (string.IsNullOrEmpty(pixelSelectorDto.SecretKey))
        {
            throw new ArgumentNullException(null, "Секретный элемент не может быть пустым!");
        }

        #endregion

        var pixels = pixelSelectorDto.Pixels;
        var random = new Random(pixelSelectorDto.SecretKey.GetHashCode());
        var usedPixels = new List<int>();

        while (true)
        {
            // Сгенерированный индекс пикселя.
            var index = random.Next(pixels.Length - 1);
            
            // Если ранее данный пиксель уже был сгенерирован.
            if (usedPixels.Contains(index)) continue;

            // Добавление пикселя в колекцию использованных.
            usedPixels.Add(index);

            // Возврат идентификатора пикселя.
            yield return index;
        }
        
    }
}