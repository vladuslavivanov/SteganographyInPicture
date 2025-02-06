using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteganographyInPicture.Services.Implementations;
using SteganographyInPicture.Steganography.Implementations;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using SteganographyInPicture.Enums;
using System.Collections.ObjectModel;
using static SteganographyInPicture.Extensions.ImageExtension;
using System.Runtime.CompilerServices;

namespace SteganographyInPicture.UI.Pages.ViewModel.Implementations;

public partial class EraseImagePageViewModel : ObservableObject
{
    public EraseImagePageViewModel()
    {
        SelectedExtension = AvailableExtensions[0];
    }

    [ObservableProperty]
    private string pathToImage = "";

    [ObservableProperty]
    private int encodingDepth;

    [ObservableProperty]
    private BitmapImage? sourceImage;

    [ObservableProperty]
    private BitmapImage? resultImage;

    [ObservableProperty]
    private Image? resultImageToSave;

    [ObservableProperty]
    private ObservableCollection<ImageExtensionsEnum> availableExtensions =
        new(Enum.GetValues<ImageExtensionsEnum>());

    [ObservableProperty]
    private ImageExtensionsEnum selectedExtension;

    [RelayCommand]
    async Task OpenFile()
    {
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.OpenImage();
        PathToImage = path ?? "";
        SourceImage = string.IsNullOrEmpty(PathToImage) ? null : new(new Uri(PathToImage));
    }

    [RelayCommand]
    async Task ClearBits()
    {
        var cleanPixels = new CleanPixels();

        var cleanImage = cleanPixels.CleanImage(Image.Load(PathToImage), EncodingDepth);

        ResultImage = await cleanImage.SaveAsImageAsync(SelectedExtension);

        ResultImageToSave = cleanImage;
    }

    [RelayCommand]
    async Task SaveImage()
    {
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.SaveUs(new() { SelectedExtension.ToString().Insert(0, ".") });

        if (string.IsNullOrEmpty(path)) return;

        ResultImageToSave!.Save(path);
    }

    
    partial void OnSourceImageChanged(BitmapImage? value)
    {
        ResultImage = null;
        ResultImageToSave = null;
    }

}
