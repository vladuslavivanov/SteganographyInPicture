using SixLabors.ImageSharp;

namespace SteganographyInPicture.DTO;

public record EncryptPhotoResultDto(Image imageResult, int frequencyOfGroups = 0);