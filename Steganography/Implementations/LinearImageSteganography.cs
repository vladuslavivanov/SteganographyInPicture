using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Consts;
using SteganographyInPicture.DTO;
using SteganographyInPicture.Steganography.Interfaces;
using System;
using System.Collections.Generic;

namespace SteganographyInPicture.Steganography.Implementations;

internal class LinearImageSteganography : IImageSteganography
{

    #region Singleton

    private LinearImageSteganography() { }

    private static readonly Lazy<LinearImageSteganography> _instance = 
        new(() => new());

    public static LinearImageSteganography Instance => _instance.Value;

    #endregion

    public string DecryptPhoto(DecryptPhotoDto decryptPhotoDto)
    {
        var result =
            ImageSteganography.DecryptPhoto(decryptPhotoDto, PixelSelector);

        return result.Remove(result.Length - 1);
    }

    public EncryptPhotoResultDto EncryptPhoto(EncryptPhotoDto encryptPhotoDto)
    {
        return ImageSteganography.EncryptPhoto(encryptPhotoDto, PixelSelector);
    }

    IEnumerable<int> PixelSelector(PixelSelectorDto pixelSelectorDto)
    {
        var pixels = pixelSelectorDto.Pixels;

        for (int i = 0; i < pixels.Length; i++)
        {
            yield return i;
        }
    }
}