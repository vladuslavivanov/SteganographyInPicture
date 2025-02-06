using System;
using System.Collections;
using System.Collections.Generic;

namespace SteganographyInPicture.Enumerators;

internal class BitEnumerator : IEnumerator<bool>
{
    public BitEnumerator(byte[] array)
    {
        _bytes = array;
    }

    byte[] _bytes;
    int _position = -1;
    int _readedBits = -1;

    public bool Current
    {
        get
        {
            try
            {
                return (_bytes[_position] & (1 << 7 - _readedBits)) > 0 ? true : false;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
    }

    object IEnumerator.Current => Current;

    public void Dispose() { }

    public bool MoveNext()
    {
        if (_position == -1 && _readedBits == -1)
        {
            _readedBits++;
            return ++_position < (_bytes.Length);
        }

        if (++_readedBits > 7)
        {
            _readedBits = 0;
            return ++_position < _bytes.Length;
        }

        return true;
    }

    public void Reset()
    {
        _position = -1;
        _readedBits = -1;
    }
}
