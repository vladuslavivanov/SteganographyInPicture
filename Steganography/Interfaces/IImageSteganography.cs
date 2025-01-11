using SixLabors.ImageSharp;
using SteganographyInPicture.DTO;
using System.Threading.Tasks;

namespace SteganographyInPicture.Steganography.Interfaces;

internal interface IImageSteganography
{
    /// <summary>
    /// Скрыть информацию в фото.
    /// </summary>
    Task<EncryptPhotoResultDto> EncryptPhotoAsync(EncryptPhotoDto decryptPhotoDto);

    /// <summary>
    /// Получить секретную информацию с фото.
    /// </summary>
    string DecryptPhoto(DecryptPhotoDto encryptPhotoDto);
}
