using System;

namespace SteganographyInPicture.Services.Interfaces;

interface IEncryptedKeyService
{
    string CreateSHA256Hash(string input);
    Guid GetGuid();
}
