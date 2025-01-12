using SixLabors.ImageSharp;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Models;
using System.Collections.Generic;

namespace SteganographyInPicture.DTO;

/// <param name="Image">Фото.</param>
/// <param name="PixelsBitDepth">Количество битов для встраивания в каждый каналы пикселей.</param>
/// <param name="QuantityPixelsInGroup">Количество пикселей в группе пикселей.</param>
/// <param name="FrequencyOfGroups">Частота групп.</param>
/// <param name="SecretKey">Секретный ключ.</param>
/// <param name="Encoding">Кодировка текста.</param>
/// <param name="Compression">Тип сжатия.</param>
internal record DecryptPhotoDto(
    Image Image, 
    EncodingEnum Encoding,
    IEnumerable<PixelChannelModel> PixelsBitDepth, 
    int QuantityPixelsInGroup, 
    int FrequencyOfGroups, 
    string SecretKey,
    CompressionsEnum Compression);
