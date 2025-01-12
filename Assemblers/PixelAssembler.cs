using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteganographyInPicture.Assemblers;

internal class PixelAssembler
{
    public PixelAssembler(IEnumerable<PixelChannelModel> pixelChannels)
    {
        _pixelChannelsEnumerator = pixelChannels.GetEnumerator();
        _pixelChannels = pixelChannels;
    }

    IEnumerator<PixelChannelModel> _pixelChannelsEnumerator;
    readonly IEnumerable<PixelChannelModel> _pixelChannels;

    Rgb24 currentPixel = new();
    int quntityBitsToWrite;
    int quantityWritedBits;

    public List<Rgb24> ArrayOfPixels { get; init; } = new();

    public void AddBit(bool bit)
    {
        if (quntityBitsToWrite == quantityWritedBits)
        {
            if (!_pixelChannelsEnumerator.MoveNext())
            {
                _pixelChannelsEnumerator = _pixelChannels.GetEnumerator();
                _pixelChannelsEnumerator.MoveNext();
                ArrayOfPixels.Add(currentPixel);
                currentPixel = new Rgb24();
            }
            quntityBitsToWrite = _pixelChannelsEnumerator.Current.EncodingDepth;
            quantityWritedBits = 0;
        }

        switch (_pixelChannelsEnumerator.Current.PixelChannel)
        {
            case Enums.PixelChannelsEnum.R:
                currentPixel.R <<= 1;
                currentPixel.R |= Convert.ToByte(bit);
                break;
            case Enums.PixelChannelsEnum.G:
                currentPixel.G <<= 1;
                currentPixel.G |= Convert.ToByte(bit);
                break;
            case Enums.PixelChannelsEnum.B:
                currentPixel.B <<= 1;
                currentPixel.B |= Convert.ToByte(bit);
                break;
            default:
                break;
        }
        quantityWritedBits++;
    }

    public void AddLastPixelIfNotExist()
    {
        if (ArrayOfPixels.Last() != currentPixel)
        {
            switch (_pixelChannelsEnumerator.Current.PixelChannel)
            {
                case Enums.PixelChannelsEnum.R:
                    currentPixel.R <<= 1;
                    break;
                case Enums.PixelChannelsEnum.G:
                    currentPixel.G <<= 1;
                    break;
                case Enums.PixelChannelsEnum.B:
                    currentPixel.B <<= 1;
                    break;
                default:
                    break;
            }
            ArrayOfPixels.Add(currentPixel);
        }
    }
}
