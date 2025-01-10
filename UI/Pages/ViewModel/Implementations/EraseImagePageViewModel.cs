using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteganographyInPicture.Services.Implementations;
using SteganographyInPicture.Steganography.Implementations;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using SteganographyInPicture.DTO;

namespace SteganographyInPicture.UI.Pages.ViewModel.Implementations;

public partial class EraseImagePageViewModel : ObservableObject
{
    [ObservableProperty]
    private string pathToImage;

    [ObservableProperty]
    private int encodingDepth;

    [RelayCommand]
    async Task OpenFile()
    {
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.OpenImage();
        PathToImage = path ?? "";
    }

    [RelayCommand]
    async Task ClearBits()
    {
        var cleanPixels = new CleanPixels();

        var cleanImage = cleanPixels.CleanImage(Image.Load(PathToImage), EncodingDepth);

        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.SaveUs(new() {".png", ".jpeg", ".tiff", ".bmp"});

        if (string.IsNullOrEmpty(path)) return;
        
        await cleanImage.SaveAsync(path);
    }
}
