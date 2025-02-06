using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using SteganographyInPicture.UI.Pages.View;
using SteganographyInPicture.Models;
using System;
using SteganographyInPicture.Services.Implementations;

namespace SteganographyInPicture.UI.Windows.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        selectedItem = menuItems[0];
        if (SecurityAccessService.IsUserAdministrator())
        {
            footerMenuItems.Add(new(new SettingsPage(), "Настройки", "\uE713"));
        }
    }

    [ObservableProperty]
    List<MenuItemModel> menuItems = new()
    {
        new(new EncryptPage(), "Кодирование", "\uE72E"),
        new(new DecryptPage(), "Декодирование", "\uE785"),
        new(new CalculatorPage(), "Ручной расчет", "\uE8EF"),
        new(new EraseImagePage(), "Удалить информацию", "\uED61"),
    };

    [ObservableProperty]
    List<MenuItemModel> footerMenuItems = new()
    {
        new(new StudyPage(), "Обучение", "\uEA80")
    };

    [ObservableProperty]
    MenuItemModel selectedItem;

    [RelayCommand]
    void Navigate(NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer != null &&
            args.InvokedItemContainer.Tag != null &&
            args.InvokedItemContainer.Tag is MenuItemModel)
        {
            SelectedItem = args.InvokedItemContainer.Tag as MenuItemModel ??
                throw new Exception();
            return;
        }
        SelectedItem = args.InvokedItem as MenuItemModel ??
            throw new Exception();
    }
}
