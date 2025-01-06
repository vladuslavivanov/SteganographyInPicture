using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using SixLabors.ImageSharp;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Factories;
using SteganographyInPicture.Services.Implementations;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SteganographyInPicture.UI.Pages.ViewModel.Implementations;

internal partial class DecryptPageViewModel : ObservableObject
{
    public DecryptPageViewModel()
    {
        OpenFileCommand = new AsyncRelayCommand(OpenFile);
    }

    [ObservableProperty]
    private int encodingDepth = 1;

    [ObservableProperty]
    private ImageSteganographyMethodEnum selectedMethod =
        ImageSteganographyMethodEnum.Linear;

    [ObservableProperty]
    private EncodingEnum selectedEncoding;

    [ObservableProperty]
    private ObservableCollection<EncodingEnum> availableEncodings =
        new(Enum.GetValues<EncodingEnum>());

    [ObservableProperty]
    private string secretKey = "";

    [ObservableProperty]
    private int? quantityPixelsInGroups;

    [ObservableProperty]
    private int? frequencyOfGroups;

    [ObservableProperty]
    private string decryptedText = "";

    [ObservableProperty]
    private string pathToImage = "";

    [ObservableProperty]
    private Visibility secretKeyVisible = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility quantityPixelsInGroupsVisible = Visibility.Collapsed;

    public IAsyncRelayCommand OpenFileCommand { get; }

    async Task OpenFile()
    {
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.OpenImage();
        if (path is not null)
            PathToImage = path;
    }

    [RelayCommand]
    void DecryptImage()
    {
        var imageSteganographyFactory = new ImageSteganographyFactory();
        var instanse = imageSteganographyFactory.GetPhotoSteganography(SelectedMethod);

        var text = instanse.DecryptPhoto(new(Image.Load(PathToImage), SelectedEncoding, EncodingDepth, (int)QuantityPixelsInGroups, 271151, SecretKey));
        DecryptedText = text;
    }

    partial void OnSelectedMethodChanged(ImageSteganographyMethodEnum value)
    {
        _ = value == ImageSteganographyMethodEnum.Pseudorandom ?
            SecretKeyVisible = Visibility.Visible :
            SecretKeyVisible = Visibility.Collapsed;

        _ = value == ImageSteganographyMethodEnum.Uniform ?
            QuantityPixelsInGroupsVisible = Visibility.Visible :
            QuantityPixelsInGroupsVisible = Visibility.Collapsed;
    }
}
