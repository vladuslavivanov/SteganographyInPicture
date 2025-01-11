using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SixLabors.ImageSharp;
using SteganographyInPicture.DTO;
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
    private CompressionsEnum selectedCompression;

    [ObservableProperty]
    private ObservableCollection<CompressionsEnum> availableCompressions =
        new(Enum.GetValues<CompressionsEnum>());

    [ObservableProperty]
    private string secretKey = "";

    [ObservableProperty]
    private int quantityPixelsInGroups = 1;

    [ObservableProperty]
    private int frequencyOfGroups;

    [ObservableProperty]
    private string pathToImage;

    [ObservableProperty]
    private string textToHide = "";

    [ObservableProperty]
    private Visibility secretKeyVisible = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility quantityPixelsInGroupsVisible = Visibility.Collapsed;

    public ObservableCollection<InfoBar> InfoBarMessages { get; set; } = new();

    [RelayCommand]
    async Task OpenFile()
    {
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.OpenImage();
        PathToImage = path ?? "";
    }

    [RelayCommand]
    void GenerateGuid()
    {
        var encryptedKeyService = new EncryptedKeyService();
        SecretKey = encryptedKeyService.GetGuid().ToString();
    }

    [RelayCommand]
    async Task EncryptImage()
    {
        var imageSteganographyFactory = new ImageSteganographyFactory();
        var instanse = imageSteganographyFactory.GetPhotoSteganography(SelectedMethod);

        EncryptPhotoResultDto encryptPhotoResultDto;
        var infoBarService = new InfoBarService();

        try
        {
            encryptPhotoResultDto = await instanse.EncryptPhotoAsync(new(SixLabors.ImageSharp.Image.Load(PathToImage), TextToHide, SelectedEncoding, EncodingDepth, QuantityPixelsInGroups, SecretKey, SelectedCompression));
        }
        catch(Exception ex)
        {
            var infoBar = infoBarService.GetInfoBar("Ошибка!", ex.Message, InfoBarSeverity.Error);
            infoBar.Closed += (sender, obj) =>
            {
                InfoBarMessages.Remove(sender);
            };
            InfoBarMessages.Add(infoBar);
            return;
        }

        FrequencyOfGroups = encryptPhotoResultDto.frequencyOfGroups;

        var exstensionImage = PathToImage.Substring(PathToImage.LastIndexOf('.')) ?? "";
        
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.SaveUs(new() { exstensionImage! });

        if (string.IsNullOrEmpty(path)) return;

        encryptPhotoResultDto.imageResult.Save(path);        
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