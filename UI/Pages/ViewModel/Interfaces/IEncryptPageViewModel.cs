using CommunityToolkit.Mvvm.Input;
using SteganographyInPicture.Enums;

namespace SteganographyInPicture.Pages.ViewModel.Interfaces;

interface IEncryptPageViewModel
{
    /// <summary>
    /// Глубина внедрения текста.
    /// </summary>
    //int EncodingDepth { get; set; }

    /// <summary>
    /// Метод стеганографии.
    /// </summary>
    ImageSteganographyMethodEnum SelectedMethod { get; set; }
    
    /// <summary>
    /// Секретный элемент.
    /// </summary>
    string SecretKey { get; set; }
    
    /// <summary>
    /// Путь к исходной картинке.
    /// </summary>
    string PathToImage { get; set; }

    /// <summary>
    /// Открытие картинки.
    /// </summary>
    IAsyncRelayCommand OpenFileCommand { get; }

    /// <summary>
    /// Стеганографирование.
    /// </summary>
    IAsyncRelayCommand EncryptImageCommand { get; }
}
