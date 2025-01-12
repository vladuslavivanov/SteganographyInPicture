using SteganographyInPicture.Enums;

namespace SteganographyInPicture.Models;

public class PixelChannelModel
{
    int encodingDepth;

    public PixelChannelsEnum PixelChannel { get; init; }
    public int EncodingDepth 
    {
        get => encodingDepth;
        set
        {
            if (value < 0 || value > 8) return;
            encodingDepth = value;
        }
    }
}