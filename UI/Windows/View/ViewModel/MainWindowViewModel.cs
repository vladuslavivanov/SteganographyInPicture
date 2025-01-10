using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using SteganographyInPicture.UI.Pages.View;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation.Collections;

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
        new StudyPage(),
        new EraseImagePage(),
    };

    [ObservableProperty]
    private Page frame = new();

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
            case "Обучение":
                Frame = lists[2];
                break;
            case "Удалить информацию":
                Frame = lists[3];
                break;
        }
    }
}
