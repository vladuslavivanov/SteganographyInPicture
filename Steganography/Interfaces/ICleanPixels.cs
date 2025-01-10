using SixLabors.ImageSharp;

namespace SteganographyInPicture.Steganography.Interfaces;

interface ICleanPixels
{
    Image CleanImage(Image image, int depth);
}
