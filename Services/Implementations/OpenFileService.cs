using Microsoft.UI.Xaml.Controls;
using SteganographyInPicture.Enums;
using SteganographyInPicture.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SteganographyInPicture.Services.Implementations;

class OpenFileService : IOpenFileService
{
    public async Task<string?> OpenFile(List<string>? extensions)
    {
        var picker = new FileOpenPicker();

        // See the sample code below for how to make the window accessible from the App class.
        var window = App.Window;

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

        // Добавление фильтров
        extensions?.ForEach(picker.FileTypeFilter.Add);

        // Показываем диалог и получаем выбранный файл
        StorageFile? file = await picker.PickSingleFileAsync();

        return file?.Path;
    }

    public async Task<string?> OpenImage()
    {
        var extensions = Enum.GetNames(typeof(ImageExtensionsEnum)).Select(e => "." + e).ToList();

        return await OpenFile(extensions);
    }

    public async Task<string?> SaveUs(List<string>? availableExstensions = null)
    {
        var picker = new FileSavePicker();

        // See the sample code below for how to make the window accessible from the App class.
        var window = App.Window;

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

        if(availableExstensions is null)
            availableExstensions = 
                Enum.GetNames(typeof(ImageExtensionsEnum)).Select(e => "." + e).ToList();
        

        picker.FileTypeChoices.Add(new("Изображение", availableExstensions));

        var file = await picker.PickSaveFileAsync();
        
        return file?.Path;
    }

}
