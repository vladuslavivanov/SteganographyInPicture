using Windows.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteganographyInPicture.UI.Constants;
using SteganographyInPicture.Services.Implementations;
using System.Threading.Tasks;

namespace SteganographyInPicture.UI.Pages.ViewModel.Implementations;

internal partial class SettingsPageViewModel : ObservableObject
{
    public SettingsPageViewModel()
    {
        localSettings.Values.TryGetValue(
            StudyPageConstants.PATH_TO_STUDY_HTML, out object? result);

        PathToStudyHtml = result as string ?? "";
    }

    ApplicationDataContainer localSettings = 
        ApplicationData.Current.LocalSettings;

    [ObservableProperty]
    string pathToStudyHtml;

    [RelayCommand]
    async Task SetPathToStudyHtml()
    {
        OpenFileService openFileService = new OpenFileService();
        var path = await openFileService.OpenFile(new() { ".html" });

        if (string.IsNullOrEmpty(path))
            return;

        PathToStudyHtml = path;

        localSettings.Values[StudyPageConstants.PATH_TO_STUDY_HTML] = PathToStudyHtml;
    }
}
