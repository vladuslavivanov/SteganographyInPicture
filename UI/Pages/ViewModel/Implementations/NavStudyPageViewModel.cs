using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteganographyInPicture.Services.Implementations;
using SteganographyInPicture.UI.Constants;
using System;
using Windows.Storage;

namespace SteganographyInPicture.UI.Pages.ViewModel.Implementations;

public partial class NavStudyPageViewModel : ObservableObject
{
    public NavStudyPageViewModel()
    {
        localSettings.Values.TryGetValue(
            StudyPageConstants.PATH_TO_STUDY_HTML, out object? result);

        PathToStudyHtml = new(result as string ?? "");
    }

    ApplicationDataContainer localSettings =
        ApplicationData.Current.LocalSettings;

    [ObservableProperty]
    Uri pathToStudyHtml;

    [ObservableProperty]
    bool isUserAdmin = SecurityAccessService.IsUserAdministrator();

    [RelayCommand]
    void RefreshHTML()
    {
        localSettings.Values.TryGetValue(
            StudyPageConstants.PATH_TO_STUDY_HTML, out object? result);

        PathToStudyHtml = null!;
        PathToStudyHtml = new(result as string ?? "");
    }
}
