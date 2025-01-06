using SixLabors.ImageSharp;
using SteganographyInPicture.DTO;

namespace SteganographyInPicture.Steganography.Interfaces;

internal interface IImageSteganography
{
    /// <summary>
    /// Скрыть информацию в фото.
    /// </summary>
    Image EncryptPhoto(EncryptPhotoDto decryptPhotoDto);

    /// <summary>
    /// Получить секретную информацию с фото.
    /// </summary>
    string DecryptPhoto(DecryptPhotoDto encryptPhotoDto);
}
