namespace SteganographyInPicture.Enums;

public enum ImageSteganographyMethodEnum
{
    /// <summary>
    /// Линейное распределение битов.
    /// </summary>
    Linear = 1,

    /// <summary>
    /// Равномерное распределение битов.
    /// </summary>
    Uniform = 2,

    /// <summary>
    /// Псевдослучайное распределение битов.
    /// </summary>
    Pseudorandom = 3,
}
