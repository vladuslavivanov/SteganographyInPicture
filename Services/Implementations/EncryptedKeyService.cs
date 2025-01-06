using SteganographyInPicture.Services.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SteganographyInPicture.Services.Implementations;

class EncryptedKeyService : IEncryptedKeyService
{
    public string CreateSHA256Hash(string input)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder builder = new StringBuilder();
        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }

    public Guid GetGuid()
    {
        return Guid.NewGuid();
    }

}
