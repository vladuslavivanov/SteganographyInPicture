using SteganographyInPicture.Enums;
using System;
using System.Collections.Generic;

namespace SteganographyInPicture.Consts;

public static class EncodingConsts
{
    public const byte MARKER_END_OF_STRING = 0x0;
    public const int QUANTITY_BIT_IN_BYTE = 8;
    public const int CHANNEL_COUNT = 3;

    public static List<byte> GetEndOfStringSymbols(EncodingEnum encoding)
    {
        return encoding switch
        {
            EncodingEnum.UTF8 => new() { MARKER_END_OF_STRING },
            EncodingEnum.UNICODE => new() { MARKER_END_OF_STRING, MARKER_END_OF_STRING },
            EncodingEnum.UTF32 => new() { MARKER_END_OF_STRING, MARKER_END_OF_STRING, MARKER_END_OF_STRING, MARKER_END_OF_STRING },
            EncodingEnum.ASCII => new() { MARKER_END_OF_STRING },
            _ => throw new ArgumentOutOfRangeException(nameof(encoding))
        };
    }
}
