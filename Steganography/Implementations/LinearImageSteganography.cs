using SteganographyInPicture.DTO;
using SteganographyInPicture.Steganography.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteganographyInPicture.Steganography.Implementations;

internal class LinearImageSteganography : IImageSteganography
{

    #region Singleton

    private LinearImageSteganography() { }

    private static readonly Lazy<LinearImageSteganography> _instance = 
        new(() => new());

    public static LinearImageSteganography Instance => _instance.Value;

    #endregion

    public string DecryptPhoto(DecryptPhotoDto decryptPhotoDto) =>
        ImageSteganography.DecryptPhoto(decryptPhotoDto, PixelSelector);

    public async Task<EncryptPhotoResultDto> EncryptPhotoAsync(EncryptPhotoDto encryptPhotoDto) =>
        await ImageSteganography.EncryptPhotoAsync(encryptPhotoDto, PixelSelector);

    IEnumerable<int> PixelSelector(PixelSelectorDto pixelSelectorDto)
    {
        var pixels = pixelSelectorDto.Pixels;

        for (int i = 0; i < pixels.Length; i++)
        {
            yield return i;
        }
    }
}