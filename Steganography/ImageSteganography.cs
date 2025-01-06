using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Consts;
using SteganographyInPicture.DTO;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteganographyInPicture.Steganography;

internal class ImageSteganography
{
    public static Image EncryptPhoto(EncryptPhotoDto encryptPhotoDto, Func<PixelSelectorDto, IEnumerable<int>> pixelSelector)
    {
        #region Валидация

        if (encryptPhotoDto.BitDepth < 1)
        {
            throw new ArgumentException(nameof(encryptPhotoDto.BitDepth), "Глубина встраивания не может быть менее 1.");
        }

        #endregion

        // Текст с терминатором.
        var text = encryptPhotoDto.Text + '\0';

        // Копия изображения.
        var cloneImage = encryptPhotoDto.Image.CloneAs<Rgb24>();

        // Массив пикселей.
        var arrayOfPixels = new Rgb24[cloneImage.Width * cloneImage.Height];

        // Копирование пикселей изображения в массив пикселей.
        cloneImage.CopyPixelDataTo(arrayOfPixels);

        // Пиксели только с сокрытыми данными.
        var textInPixels = new ConverterService().ConvertTextToRgb24Array(text, encryptPhotoDto.BitDepth, encryptPhotoDto.Encoding).ToArray();

        if (textInPixels.Count() > arrayOfPixels.Length)
        {
            throw new InvalidOperationException("Количество текста в пикселях превышает размер массива пикселей.");
        }

        var zeroingMask = ~((1 << encryptPhotoDto.BitDepth) - 1);

        var frequencyOfGroups = () => 
        {
            if (encryptPhotoDto.QuantityPixelsInGroup < 1) return 0;

            // Количество групп.
            var quantityGroups = (int)Math.Round((double)textInPixels.Length / encryptPhotoDto.QuantityPixelsInGroup, MidpointRounding.ToPositiveInfinity);

            // Частота групп.
            return (int)Math.Round((double)arrayOfPixels.Length / quantityGroups, MidpointRounding.AwayFromZero);
        };

        var pixelAlgorithm = pixelSelector(new(arrayOfPixels, encryptPhotoDto.QuantityPixelsInGroup, frequencyOfGroups(), encryptPhotoDto.SecretKey))
            .GetEnumerator();

        for (int i = 0; i < textInPixels.Length; i++)
        {
            var dataToHide = textInPixels[i];
            
            pixelAlgorithm.MoveNext();

            for (int chanel = 0; chanel < EncodingConsts.CHANNEL_COUNT; chanel++)
            {
                _ = chanel switch
                {
                    0 => arrayOfPixels[pixelAlgorithm.Current].R = (byte)(arrayOfPixels[pixelAlgorithm.Current].R & zeroingMask | dataToHide.R),
                    1 => arrayOfPixels[pixelAlgorithm.Current].G = (byte)(arrayOfPixels[pixelAlgorithm.Current].G & zeroingMask | dataToHide.G),
                    2 => arrayOfPixels[pixelAlgorithm.Current].B = (byte)(arrayOfPixels[pixelAlgorithm.Current].B & zeroingMask | dataToHide.B),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        // Изображение с зашифрованной информацией.
        var newImage = Image.LoadPixelData(new ReadOnlySpan<Rgb24>(arrayOfPixels), cloneImage.Width, cloneImage.Height);

        return newImage;
    }

    public static string DecryptPhoto(DecryptPhotoDto decryptPhotoDto, Func<PixelSelectorDto, IEnumerable<int>> pixelSelector)
    {
        #region Валидация

        if (decryptPhotoDto.BitDepth < 1)
        {
            throw new ArgumentException(nameof(decryptPhotoDto.BitDepth), "Глубина встраивания не может быть менее 1.");
        }

        #endregion

        var converterService = new ConverterService();

        var image = decryptPhotoDto.Image.CloneAs<Rgb24>();
        
        var pixelsSpan = new Rgb24[image.Width * image.Height];
        image.CopyPixelDataTo(pixelsSpan);

        var arrayOfBytes = new List<byte>();
        var symbol = default(byte);
        var countBits = default(int);

        foreach (var iteratorOfPixel in pixelSelector(new(pixelsSpan, decryptPhotoDto.QuantityPixelsInGroup, decryptPhotoDto.FrequencyOfGroups, decryptPhotoDto.SecretKey)))
        {
            var pixel = pixelsSpan[iteratorOfPixel];

            for (int i = 0; i < EncodingConsts.CHANNEL_COUNT; i++)
            {
                var channel = i switch
                {
                    0 => pixel.R,
                    1 => pixel.G,
                    2 => pixel.B,
                    _ => throw new ArgumentOutOfRangeException()
                };

                // Извлекаем биты из канала
                for (int j = decryptPhotoDto.BitDepth - 1; j >= 0; j--)
                {
                    var bit = ((channel & (1 << j)) != 0) ?
                        (byte)1 : (byte)0;
                    symbol |= bit;

                    if (++countBits == EncodingConsts.QUANTITY_BIT_IN_BYTE)
                    {
                        arrayOfBytes.Add(symbol);
                        countBits = 0;
                        symbol = default;

                        // Проверка маркера конца текста.
                        if (symbol == EncodingConsts.MARKER_END_OF_STRING &&
                            converterService.CheckArrayOfBytesOnNullTermenator(arrayOfBytes, decryptPhotoDto.Encoding))
                        {
                            return decryptPhotoDto.Encoding switch
                            {
                                EncodingEnum.UTF8 => Encoding.UTF8.GetString(arrayOfBytes.ToArray()),
                                EncodingEnum.UNICODE => Encoding.Unicode.GetString(arrayOfBytes.ToArray()),
                                EncodingEnum.UTF32 => Encoding.UTF32.GetString(arrayOfBytes.ToArray()),
                                EncodingEnum.ASCII => Encoding.ASCII.GetString(arrayOfBytes.ToArray()),
                                _ => throw new ArgumentOutOfRangeException(nameof(decryptPhotoDto.Encoding)),
                            };
                        }
                    }
                    symbol <<= 1;
                }
            }
        }

        return decryptPhotoDto.Encoding switch
        {
            EncodingEnum.UTF8 => Encoding.UTF8.GetString(arrayOfBytes.ToArray()),
            EncodingEnum.UNICODE => Encoding.Unicode.GetString(arrayOfBytes.ToArray()),
            EncodingEnum.UTF32 => Encoding.UTF32.GetString(arrayOfBytes.ToArray()),
            EncodingEnum.ASCII => Encoding.ASCII.GetString(arrayOfBytes.ToArray()),
            _ => throw new ArgumentOutOfRangeException(nameof(decryptPhotoDto.Encoding)),
        };

    }
}