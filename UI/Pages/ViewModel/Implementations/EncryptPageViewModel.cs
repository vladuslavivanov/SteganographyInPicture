using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using SixLabors.ImageSharp;
using SteganographyInPicture.DTO;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Factories;
using SteganographyInPicture.Pages.ViewModel.Interfaces;
using SteganographyInPicture.Services.Implementations;
using SteganographyInPicture.UI.CustomControls.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using static SteganographyInPicture.Extensions.ImageExtension;

namespace SteganographyInPicture.Pages.ViewModel.Implementations;

public partial class EncryptPageViewModel : ObservableObject, IEncryptPageViewModel
{
    public EncryptPageViewModel()
    {
        selectedEncoding = availableEncodings.FirstOrDefault();
    }

    [ObservableProperty]
    private string secretKey = "";

    [ObservableProperty]
    private int quantityPixelsInGroups = 1;

    [ObservableProperty]
    private int frequencyOfGroups;

    [ObservableProperty]
    private string pathToImage = "";

    [ObservableProperty]
    private string textToHide = "";

    [ObservableProperty]
    private ImageSteganographyMethodEnum selectedMethod = 
        ImageSteganographyMethodEnum.Linear;

    [ObservableProperty]
    private ObservableCollection<EncodingEnum> availableEncodings =
        new(Enum.GetValues<EncodingEnum>());

    [ObservableProperty]
    private EncodingEnum selectedEncoding;

    [ObservableProperty]
    private ObservableCollection<CompressionsEnum> availableCompressions =
        new(Enum.GetValues<CompressionsEnum>());

    [ObservableProperty]
    private CompressionsEnum selectedCompression;

    [ObservableProperty]
    private ObservableCollection<PixelChannelControlViewModel> availablePixelChannels =
        new(Enum.GetValues<PixelChannelsEnum>()
            .Select(p => new PixelChannelControlViewModel(new() { PixelChannel = p })));

    [ObservableProperty]
    private Visibility secretKeyVisible = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility quantityPixelsInGroupsVisible = Visibility.Collapsed;

    [ObservableProperty]
    private BitmapImage sourceImage;

    [ObservableProperty]
    private BitmapImage resultImage;

    [ObservableProperty]
    private SixLabors.ImageSharp.Image? resultImageToSave;

    public ObservableCollection<InfoBar> InfoBarMessages { get; set; } = new();

    [RelayCommand]
    async Task OpenFile()
    {
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.OpenImage();
        PathToImage = path ?? "";
        SourceImage = string.IsNullOrEmpty(PathToImage) ? new() : new(new Uri(PathToImage));
        ResultImage = new();
        ResultImageToSave = null;
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

        InfoBar infoBar;

        try
        {
            encryptPhotoResultDto = await instanse.EncryptPhotoAsync(new(SixLabors.ImageSharp.Image.Load(PathToImage), TextToHide, SelectedEncoding, AvailablePixelChannels.Select(p => p.Model), QuantityPixelsInGroups, SecretKey, SelectedCompression));
        }
        catch(Exception ex)
        {
            infoBar = infoBarService.GetInfoBar("Ошибка!", ex.Message, InfoBarSeverity.Error);
            infoBar.Closed += (sender, obj) =>
            {
                InfoBarMessages.Remove(sender);
            };
            InfoBarMessages.Add(infoBar);
            return;
        }

        var exstensionImage = PathToImage.Substring(PathToImage.LastIndexOf('.')) ?? 
            throw new ArgumentNullException("Ошибка чтения расширения изображения.");

        Enum.TryParse<ImageExtensionsEnum>(exstensionImage.Substring(1), out var imageExtension);

        ResultImage = await encryptPhotoResultDto.imageResult.SaveAsImageAsync(imageExtension);
        
        FrequencyOfGroups = encryptPhotoResultDto.frequencyOfGroups;
        ResultImageToSave = encryptPhotoResultDto.imageResult;

        infoBar = infoBarService.GetInfoBar("Скрытие завершено", "Информация успешно скрыта в изображении.", InfoBarSeverity.Success);
        infoBar.Closed += (sender, obj) =>
        {
            InfoBarMessages.Remove(sender);
        };
        InfoBarMessages.Add(infoBar);
        _ = RemoveInfoBarAfterDelay(infoBar, TimeSpan.FromSeconds(5));
    }

    [RelayCommand]
    async Task SaveImage()
    {
        var exstensionImage = PathToImage.Substring(PathToImage.LastIndexOf('.')) ??
            throw new ArgumentNullException("Ошибка чтения расширения изображения.");

        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.SaveUs(new() { exstensionImage });

        if (string.IsNullOrEmpty(path)) return;

        ResultImageToSave.Save(path);
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

    private async Task RemoveInfoBarAfterDelay(InfoBar infoBar, TimeSpan delay)
    {
        await Task.Delay(delay);

        if (InfoBarMessages.Contains(infoBar))
        {
            InfoBarMessages.Remove(infoBar);
        }
    }
}