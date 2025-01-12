using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Assemblers;
using SteganographyInPicture.Consts;
using SteganographyInPicture.Enumerators;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Extensions;
using SteganographyInPicture.Models;
using SteganographyInPicture.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyInPicture.Services.Implementations;

internal class ConverterService : IConverterService
{
    public async Task<List<Rgb24>> ConvertTextToRgb24ArrayAsync(string text, IEnumerable<PixelChannelModel> PixelsBitDepth, EncodingEnum encoding, CompressionsEnum compression)
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

        // Сжатие текста.
        byteArray = await byteArray.CompressBytesAsync(compression);

        // Количество байт в сжатом тексте.
        var lengthByteArray = BitConverter.GetBytes(byteArray.Length);
        
        // Количество байт в сжатом тексте + сжатый текст.
        byteArray = lengthByteArray.Concat(byteArray).ToArray();

        // Переборщик битов в массиве байтов.
        var bitsConsistently = new BitEnumerator(byteArray);

        // Сборщик пикселей.
        PixelAssembler pixelAssembler = new(PixelsBitDepth);

        // Цикл по битам.
        while (bitsConsistently.MoveNext())
        {
            pixelAssembler.AddBit(bitsConsistently.Current);
        }

        pixelAssembler.AddLastPixelIfNotExist();

        return pixelAssembler.ArrayOfPixels;

    }
}