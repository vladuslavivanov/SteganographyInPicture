using SixLabors.ImageSharp;
using SteganographyInPicture.Enums;

namespace SteganographyInPicture.DTO;

/// <param name="Image">Фото.</param>
/// <param name="Text">Тест для сокрытия.</param>
/// <param name="BitDepth">Количество битов для встраивания в каждый канал пикселя (RGB).</param>
/// <param name="QuantityPixelsInGroup">Количество пикселей в группе пикселей.</param>
/// <param name="SecretKey">Секретный ключ.</param>
internal record EncryptPhotoDto(
    Image Image, 
    string Text, 
    EncodingEnum Encoding, 
    int BitDepth, 
    int QuantityPixelsInGroup, 
    string SecretKey);