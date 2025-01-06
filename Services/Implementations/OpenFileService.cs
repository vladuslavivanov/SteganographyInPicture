using SteganographyInPicture.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

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
}
