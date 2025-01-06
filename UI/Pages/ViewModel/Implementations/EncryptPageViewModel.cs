using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using SixLabors.ImageSharp;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Factories;
using SteganographyInPicture.Pages.ViewModel.Interfaces;
using SteganographyInPicture.Services.Implementations;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SteganographyInPicture.Pages.ViewModel.Implementations;

public partial class EncryptPageViewModel : ObservableObject, IEncryptPageViewModel
{
    public EncryptPageViewModel()
    {
        OpenFileCommand = new AsyncRelayCommand(OpenFile);
        selectedEncoding = availableEncodings.FirstOrDefault();
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
    private int quantityPixelsInGroups = 1;

    [ObservableProperty]
    private string pathToImage;

    [ObservableProperty]
    private string textToHide = "Secret Text";

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
    void GenerateGuid()
    {
        var encryptedKeyService = new EncryptedKeyService();
        SecretKey = encryptedKeyService.GetGuid().ToString();
    }

    [RelayCommand]
    void EncryptImage()
    {
        var imageSteganographyFactory = new ImageSteganographyFactory();
        var instanse = imageSteganographyFactory.GetPhotoSteganography(SelectedMethod);

        var image = instanse.EncryptPhoto(new(Image.Load(PathToImage), TextToHide, SelectedEncoding, EncodingDepth, QuantityPixelsInGroups, SecretKey));        

        var path = "C:\\Users\\Vladislav\\Desktop\\image.png";
        image.Save(path);        
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
