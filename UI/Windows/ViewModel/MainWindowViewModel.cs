using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using SteganographyInPicture.UI.Pages.View;

namespace SteganographyInPicture.UI.Windows.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        frame = lists[0];
    }

    List<Page> lists = new()
    {
        new EncryptPage(),
        new DecryptPage(),
    };

    [ObservableProperty]
    private Page frame;

    [RelayCommand]
    void Navigate(NavigationViewItemInvokedEventArgs args)
    {
        switch (args.InvokedItem)
        {
            case "Кодирование":
                Frame = lists[0];
                break;

            case "Декодирование":
                Frame = lists[1];
                break;
        }
    }
}
