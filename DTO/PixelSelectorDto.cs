using SixLabors.ImageSharp.PixelFormats;

namespace SteganographyInPicture.DTO;

/// <param name="Pixels">Массив пикселей.</param>
/// <param name="QuantityPixelsInGroup">Количество пикселей в группе.</param>
/// <param name="FrequencyOfGroups">Частота групп.</param>
/// <param name="SecretKey">Секретный ключ.</param>
internal record PixelSelectorDto(
    Rgb24[] Pixels, 
    int QuantityPixelsInGroup, 
    int FrequencyOfGroups, 
    string SecretKey);
