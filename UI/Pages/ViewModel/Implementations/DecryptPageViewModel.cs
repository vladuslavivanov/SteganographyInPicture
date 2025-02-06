using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Models;
using SteganographyInPicture.Factories;
using SteganographyInPicture.Services.Implementations;
using SteganographyInPicture.UI.CustomControls.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
    private CompressionsEnum selectedCompression;

    [ObservableProperty]
    private ObservableCollection<CompressionsEnum> availableCompressions =
        new(Enum.GetValues<CompressionsEnum>());

    [ObservableProperty]
    private string secretKey = "";

    [ObservableProperty]
    private int quantityPixelsInGroups;

    [ObservableProperty]
    private int frequencyOfGroups;

    [ObservableProperty]
    private string decryptedText = "";

    [ObservableProperty]
    private string pathToImage = "";

    [ObservableProperty]
    private Visibility secretKeyVisible = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility quantityPixelsInGroupsVisible = Visibility.Collapsed;

    [ObservableProperty]
    private ObservableCollection<PixelChannelControlViewModel> availablePixelChannels =
      new(Enum.GetValues<PixelChannelsEnum>()
          .Select(p => new PixelChannelControlViewModel(new PixelChannelModel() { PixelChannel = p })));

    public IAsyncRelayCommand OpenFileCommand { get; }

    public ObservableCollection<InfoBar> InfoBarMessages { get; set; } = new();

    async Task OpenFile()
    {
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.OpenImage();
        if (string.IsNullOrEmpty(path))
            return;
        PathToImage = path;
    }

    [RelayCommand]
    void DecryptImage()
    {
        var imageSteganographyFactory = new ImageSteganographyFactory();
        var instanse = imageSteganographyFactory.GetPhotoSteganography(SelectedMethod);

        var infoBarService = new InfoBarService();

        var text = default(string);
        try
        {
            text = instanse.DecryptPhoto(new(SixLabors.ImageSharp.Image.Load(PathToImage), SelectedEncoding, AvailablePixelChannels.Select(p => p.Model), QuantityPixelsInGroups, FrequencyOfGroups, SecretKey, SelectedCompression));
        }
        catch (Exception ex) 
        {
            var infoBar = infoBarService.GetInfoBar("Ошибка!", ex.Message, InfoBarSeverity.Error);
            infoBar.Closed += (sender, obj) =>
            {
                InfoBarMessages.Remove(sender);
            };
            InfoBarMessages.Add(infoBar);
            return;
        }

        DecryptedText = text;
    }

    [RelayCommand]
    void EraseDecryptText()
    {
        DecryptedText = "";
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
