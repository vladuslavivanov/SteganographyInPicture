using SteganographyInPicture.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SteganographyInPicture.Services.Implementations;

class OpenFileService : IOpenFileService
{
    public async Task<string?> OpenImage()
    {
        var picker = new FileOpenPicker();

        // See the sample code below for how to make the window accessible from the App class.
        var window = App.Window;

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

        // Добавляем фильтры для изображений
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");
        picker.FileTypeFilter.Add(".bmp");
        picker.FileTypeFilter.Add(".gif");
        picker.FileTypeFilter.Add(".tiff");

        // Показываем диалог и получаем выбранный файл
        StorageFile? file = await picker.PickSingleFileAsync();

        return file?.Path;
    }

    public async Task<string?> SaveUs(List<string> availableExstensions)
    {
        var picker = new FileSavePicker();

        // See the sample code below for how to make the window accessible from the App class.
        var window = App.Window;

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

        picker.FileTypeChoices.Add(new("Изображение", availableExstensions));

        var file = await picker.PickSaveFileAsync();
        
        return file?.Path;
    }

}
