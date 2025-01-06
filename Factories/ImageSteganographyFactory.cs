using SteganographyInPicture.Enums;
using SteganographyInPicture.Steganography.Implementations;
using SteganographyInPicture.Steganography.Interfaces;
using System;

namespace SteganographyInPicture.Factories;

internal class ImageSteganographyFactory
{
    public IImageSteganography GetPhotoSteganography(ImageSteganographyMethodEnum imageSteganographyMethod)
    {
        return imageSteganographyMethod switch
        {
            ImageSteganographyMethodEnum.Linear => LinearImageSteganography.Instance,
            ImageSteganographyMethodEnum.Uniform => UniformImageSteganography.Instance,
            ImageSteganographyMethodEnum.Pseudorandom => PseudorandomImageSteganography.Instance,
            _ => throw new NotImplementedException()
        };
    }
}
