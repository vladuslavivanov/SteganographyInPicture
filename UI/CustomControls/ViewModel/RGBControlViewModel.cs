using CommunityToolkit.Mvvm.ComponentModel;
using SixLabors.ImageSharp.PixelFormats;
using Windows.UI;

namespace SteganographyInPicture.UI.CustomControls.ViewModel;

public partial class RGBControlViewModel : ObservableObject
{
    public RGBControlViewModel(Rgb24 rgb24, int numberPixel)
    {
        R = rgb24.R;
        G = rgb24.G;
        B = rgb24.B;
        NumberPixel = numberPixel;
    }

    [ObservableProperty]
    int numberPixel;

    [ObservableProperty]
    byte r;

    [ObservableProperty]
    byte g;
    
    [ObservableProperty]
    byte b;

    [ObservableProperty]
    Color colorBrush;

    partial void OnRChanged(byte value) { UpdateColorBrush(); }
    partial void OnGChanged(byte value) { UpdateColorBrush(); }
    partial void OnBChanged(byte value) { UpdateColorBrush(); }

    private void UpdateColorBrush() 
    { 
        ColorBrush = 
            Color.FromArgb(byte.MaxValue, R, G, B); 
    }
}
