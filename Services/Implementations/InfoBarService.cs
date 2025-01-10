using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;

namespace SteganographyInPicture.Services.Implementations;

internal class InfoBarService
{
    public InfoBar GetInfoBar(string title, string message, InfoBarSeverity severity)
    {
        InfoBar infoBar = new() 
        { 
            Title = title,
            Message = message,
            Severity = severity,
            IsClosable = true,
            IsOpen = true
        };
        return infoBar;
    }
}
