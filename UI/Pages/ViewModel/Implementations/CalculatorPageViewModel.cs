using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SteganographyInPicture.Services.Implementations;
using SteganographyInPicture.UI.CustomControls.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SteganographyInPicture.UI.Pages.ViewModel.Implementations;

public partial class CalculatorPageViewModel : ObservableObject
{
    List<RGBControlViewModel> allPixels = null!;
    int height = 0;
    int width = 0;

    [ObservableProperty]
    string pathToImage = "";

    [ObservableProperty]
    ObservableCollection<RGBControlViewModel> selectedPixels = new();
    
    [ObservableProperty]
    int selectedFrom = 0;

    [ObservableProperty]
    int selectedTo = 0;

    [ObservableProperty]
    int quantityPixels;

    [RelayCommand]
    async Task OpenFile()
    {
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.OpenImage();
        PathToImage = path ?? "";
        if (string.IsNullOrEmpty(PathToImage)) return;

        Image image = await Image.LoadAsync(PathToImage);
        height = image.Height;
        width = image.Width;

        var cloneImage = image.CloneAs<Rgb24>();

        var arrayOfPixels = new Rgb24[cloneImage.Width * cloneImage.Height];

        cloneImage.CopyPixelDataTo(arrayOfPixels);

        int numberPixel = 0;

        allPixels = 
            arrayOfPixels.Select(p => new RGBControlViewModel(p, numberPixel++))
            .ToList();

        SelectedPixels.Clear();

        QuantityPixels = allPixels.Count - 1;
    }

    [RelayCommand]
    void SelectPixels()
    {
        SelectedPixels.Clear();
        allPixels.GetRange(SelectedFrom, SelectedTo - SelectedFrom + 1).ForEach(SelectedPixels.Add);
    }

    [RelayCommand]
    async Task SaveUs()
    {
        OpenFileService openFileService = new OpenFileService();
        
        var exstensionImage = PathToImage.Substring(PathToImage.LastIndexOf('.')) ??
            throw new ArgumentNullException("Ошибка чтения расширения изображения.");

        var path = await openFileService.SaveUs(new() { exstensionImage! });

        if (string.IsNullOrEmpty(path)) return;

        var arrayPixels =
            allPixels.Select(p => new Rgb24(r: p.R, g:p.G, b:p.B)).ToArray();

        var image = Image.LoadPixelData(new ReadOnlySpan<Rgb24>(arrayPixels), width, height);

        await image.SaveAsync(path);
    }
}