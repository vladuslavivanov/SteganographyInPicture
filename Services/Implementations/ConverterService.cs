using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Consts;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteganographyInPicture.Services.Implementations;

internal class ConverterService : IConverterService
{
    public List<Rgb24> ConvertTextToRgb24Array(string text, int availableBitsInChannel, EncodingEnum encoding)
    {
        var answer = new List<Rgb24>();

        var byteArray = encoding switch
        {
            EncodingEnum.UTF8 => Encoding.UTF8.GetBytes(text),
            EncodingEnum.UNICODE => Encoding.Unicode.GetBytes(text),
            EncodingEnum.UTF32 => Encoding.UTF32.GetBytes(text),
            EncodingEnum.ASCII => Encoding.ASCII.GetBytes(text),
            _ => throw new ArgumentOutOfRangeException(nameof(encoding)),
        };

        // Номер текущего символа.
        var numberChar = 0;

        // Текущий символ.
        var symbol = Convert.ToInt32(byteArray[numberChar]);

        // Пиксель.
        var newPixel = new Rgb24();

        // Количество оставшихся бит в каналей (R/G/B).
        var leftAvailableBitsInChannel = availableBitsInChannel;

        // Канал пикселя.
        var pixelChannel = PixelChannelsEnum.R;

        // Выбранный канал.
        var selectedChannel = default(byte);

        var quantityReadBits = 0;

        // Маска для определения последнего (первого слева) бита символа.
        var maskForReadLastBit = 1 << 7;

        while (numberChar != byteArray.Length)
        {

            if (symbol == default && quantityReadBits == EncodingConsts.QUANTITY_BIT_IN_BYTE)
            {
                symbol = byteArray[numberChar];
                quantityReadBits = 0;
            }

            switch (pixelChannel)
            {
                case PixelChannelsEnum.R:

                    selectedChannel = newPixel.R;

                    while (leftAvailableBitsInChannel != 0 && (symbol != default || quantityReadBits != EncodingConsts.QUANTITY_BIT_IN_BYTE))
                    {
                        // Сдвиг влево канала R.
                        newPixel.R = (byte)(newPixel.R << 1);

                        // Последний (первый слева) бит символа.
                        var lastBit = (maskForReadLastBit & symbol) == 0 ? 0 : 1;

                        // Обнуляю последний (первый слева) бит и сдвигаю символ на 1 единицу влево.
                        symbol = (symbol & ~maskForReadLastBit) << 1;

                        // Приминение последнего бита.
                        newPixel.R = (byte)(newPixel.R | lastBit);

                        leftAvailableBitsInChannel--;
                        quantityReadBits++;
                    }

                    if (leftAvailableBitsInChannel == 0)
                    {
                        leftAvailableBitsInChannel = availableBitsInChannel;
                        pixelChannel = PixelChannelsEnum.G;
                    }

                    break;

                case PixelChannelsEnum.G:

                    selectedChannel = newPixel.G;

                    while (leftAvailableBitsInChannel != 0 && (symbol != default || quantityReadBits != EncodingConsts.QUANTITY_BIT_IN_BYTE))
                    {
                        // Сдвиг влево канала R.
                        newPixel.G = (byte)(newPixel.G << 1);

                        // Последний (первый слева) бит символа.
                        var lastBit = (maskForReadLastBit & symbol) == 0 ? 0 : 1;

                        // Обнуляю последний (первый слева) бит и сдвигаю символ на 1 единицу влево.
                        symbol = (symbol & ~maskForReadLastBit) << 1;

                        // Приминение последнего бита.
                        newPixel.G = (byte)(newPixel.G | lastBit);

                        leftAvailableBitsInChannel--;
                        quantityReadBits++;
                    }

                    if (leftAvailableBitsInChannel == 0)
                    {
                        leftAvailableBitsInChannel = availableBitsInChannel;
                        pixelChannel = PixelChannelsEnum.B;
                    }
                    break;

                case PixelChannelsEnum.B:
                    selectedChannel = newPixel.B;

                    while (leftAvailableBitsInChannel != 0 && (symbol != default || quantityReadBits != EncodingConsts.QUANTITY_BIT_IN_BYTE))
                    {
                        // Сдвиг влево канала R.
                        newPixel.B = (byte)(newPixel.B << 1);

                        // Последний (первый слева) бит символа.
                        var lastBit = (maskForReadLastBit & symbol) == 0 ? 0 : 1;

                        // Обнуляю последний (первый слева) бит и сдвигаю символ на 1 единицу влево.
                        symbol = (symbol & ~maskForReadLastBit) << 1;

                        // Приминение последнего бита.
                        newPixel.B = (byte)(newPixel.B | lastBit);

                        leftAvailableBitsInChannel--;
                        quantityReadBits++;
                    }

                    if (leftAvailableBitsInChannel == 0)
                    {
                        leftAvailableBitsInChannel = availableBitsInChannel;
                        pixelChannel = PixelChannelsEnum.R;
                        answer.Add(new(newPixel.R, newPixel.G, newPixel.B));
                        newPixel = new();
                    }
                    break;
                default:
                    break;
            }

            if (symbol == default && quantityReadBits == EncodingConsts.QUANTITY_BIT_IN_BYTE)
            {
                numberChar++;
            }
        }

        answer.Add(new(newPixel.R, newPixel.G, newPixel.B));

        return answer;
    }

    public List<Rgb24> ConvertTextToRgb24Arrayy(string text, int availableBitsInChannel, EncodingEnum encoding)
    {
        var answer = new List<Rgb24>();

        var byteArray = encoding switch
        {
            EncodingEnum.UTF8 => Encoding.UTF8.GetBytes(text),
            EncodingEnum.UNICODE => Encoding.Unicode.GetBytes(text),
            EncodingEnum.UTF32 => Encoding.UTF32.GetBytes(text),
            EncodingEnum.ASCII => Encoding.ASCII.GetBytes(text),
            _ => throw new ArgumentOutOfRangeException(nameof(encoding)),
        };

        // Номер текущего символа.
        var numberChar = 0;

        // Текущий символ.
        var symbol = Convert.ToInt32(byteArray[numberChar]);

        // Пиксель.
        var newPixel = new Rgb24();

        // Количество оставшихся бит в каналей (R/G/B).
        var leftAvailableBitsInChannel = availableBitsInChannel;

        // Канал пикселя.
        var pixelChannel = PixelChannelsEnum.R;

        var quantityReadBits = 0;

        // Маска для определения последнего (первого слева) бита символа.
        var maskForReadLastBit = 1 << 7;

        PixelChannelsEnum m = PixelChannelsEnum.R | PixelChannelsEnum.G;

        var getFlags = GetFlags(m).GetEnumerator();

        while (numberChar != byteArray.Length)
        {

            if (symbol == default && quantityReadBits == EncodingConsts.QUANTITY_BIT_IN_BYTE)
            {
                symbol = byteArray[numberChar];
                quantityReadBits = 0;
            }

            switch (pixelChannel)
            {
                case PixelChannelsEnum.R:

                    while (leftAvailableBitsInChannel != 0 && (symbol != default || quantityReadBits != EncodingConsts.QUANTITY_BIT_IN_BYTE))
                    {
                        // Сдвиг влево канала R.
                        newPixel.R = (byte)(newPixel.R << 1);

                        // Последний (первый слева) бит символа.
                        var lastBit = (maskForReadLastBit & symbol) == 0 ? 0 : 1;

                        // Обнуляю последний (первый слева) бит и сдвигаю символ на 1 единицу влево.
                        symbol = (symbol & ~maskForReadLastBit) << 1;

                        // Приминение последнего бита.
                        newPixel.R = (byte)(newPixel.R | lastBit);

                        leftAvailableBitsInChannel--;
                        quantityReadBits++;
                    }

                    if (leftAvailableBitsInChannel == 0)
                    {
                        leftAvailableBitsInChannel = availableBitsInChannel;
                        getFlags.MoveNext();
                        pixelChannel = getFlags.Current;
                    }

                    break;

                case PixelChannelsEnum.G:

                    while (leftAvailableBitsInChannel != 0 && (symbol != default || quantityReadBits != EncodingConsts.QUANTITY_BIT_IN_BYTE))
                    {
                        // Сдвиг влево канала R.
                        newPixel.G = (byte)(newPixel.G << 1);

                        // Последний (первый слева) бит символа.
                        var lastBit = (maskForReadLastBit & symbol) == 0 ? 0 : 1;

                        // Обнуляю последний (первый слева) бит и сдвигаю символ на 1 единицу влево.
                        symbol = (symbol & ~maskForReadLastBit) << 1;

                        // Приминение последнего бита.
                        newPixel.G = (byte)(newPixel.G | lastBit);

                        leftAvailableBitsInChannel--;
                        quantityReadBits++;
                    }

                    if (leftAvailableBitsInChannel == 0)
                    {
                        leftAvailableBitsInChannel = availableBitsInChannel;
                        getFlags.MoveNext();
                        pixelChannel = getFlags.Current;
                    }
                    break;

                case PixelChannelsEnum.B:
                    while (leftAvailableBitsInChannel != 0 && (symbol != default || quantityReadBits != EncodingConsts.QUANTITY_BIT_IN_BYTE))
                    {
                        // Сдвиг влево канала R.
                        newPixel.B = (byte)(newPixel.B << 1);

                        // Последний (первый слева) бит символа.
                        var lastBit = (maskForReadLastBit & symbol) == 0 ? 0 : 1;

                        // Обнуляю последний (первый слева) бит и сдвигаю символ на 1 единицу влево.
                        symbol = (symbol & ~maskForReadLastBit) << 1;

                        // Приминение последнего бита.
                        newPixel.B = (byte)(newPixel.B | lastBit);

                        leftAvailableBitsInChannel--;
                        quantityReadBits++;
                    }

                    if (leftAvailableBitsInChannel == 0)
                    {
                        leftAvailableBitsInChannel = availableBitsInChannel;
                        pixelChannel = PixelChannelsEnum.R;
                        answer.Add(new(newPixel.R, newPixel.G, newPixel.B));
                        newPixel = new();
                    }
                    break;
                default:
                    break;
            }

            if (symbol == default && quantityReadBits == EncodingConsts.QUANTITY_BIT_IN_BYTE)
            {
                numberChar++;
            }
        }

        answer.Add(new(newPixel.R, newPixel.G, newPixel.B));

        return answer;
    }


    static IEnumerable<PixelChannelsEnum> GetFlags(PixelChannelsEnum channels)
    {
        while (true)
        {
            foreach (PixelChannelsEnum flag in Enum.GetValues(typeof(PixelChannelsEnum)))
            {
                if (channels.HasFlag(flag))
                {
                    yield return flag;
                }
            }
        }
    }


    public bool CheckArrayOfBytesOnNullTermenator(List<byte> bytes, EncodingEnum encoding)
    {
        var endOfStringBytes = EncodingConsts.GetEndOfStringSymbols(encoding);
        
        if (endOfStringBytes.Count > bytes.Count) return false;

        var range = bytes.GetRange(bytes.Count - endOfStringBytes.Count, endOfStringBytes.Count);

        return range.SequenceEqual(endOfStringBytes);
    }
}