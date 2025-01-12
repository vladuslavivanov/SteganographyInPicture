using CommunityToolkit.Mvvm.ComponentModel;
using SteganographyInPicture.Models;

namespace SteganographyInPicture.UI.CustomControls.ViewModel;

public partial class PixelChannelControlViewModel : ObservableObject
{
    public PixelChannelControlViewModel(PixelChannelModel model)
    {
        Model = model;
    }

    public PixelChannelModel Model { get; init; }

    public string PixelChannel 
    { 
        get => Model.PixelChannel.ToString(); 
    }

    public int EncodingDepth
    {
        get => Model.EncodingDepth;
        set
        {
            Model.EncodingDepth = value;
            OnPropertyChanged(nameof(EncodingDepth));
        }
    }

}
