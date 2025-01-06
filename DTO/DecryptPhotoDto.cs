using SixLabors.ImageSharp;
using SteganographyInPicture.Enums;

namespace SteganographyInPicture.DTO;

/// <param name="Image">Фото.</param>
/// <param name="BitDepth">Количество битов для встраивания в каждый канал пикселя (RGB).</param>
/// <param name="QuantityPixelsInGroup">Количество пикселей в группе пикселей.</param>
/// <param name="FrequencyOfGroups">Частота групп.</param>
/// <param name="SecretKey">Секретный ключ.</param>
internal record DecryptPhotoDto(
    Image Image, 
    EncodingEnum Encoding, 
    int BitDepth, 
    int QuantityPixelsInGroup, 
    int FrequencyOfGroups, 
    string SecretKey);
