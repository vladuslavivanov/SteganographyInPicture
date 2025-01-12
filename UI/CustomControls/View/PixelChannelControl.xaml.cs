using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SteganographyInPicture.UI.CustomControls.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteganographyInPicture.UI.CustomControls.View
{
    public sealed partial class PixelChannelControl : UserControl
    {
        // ����������� �������� ������������
        public PixelChannelControlViewModel ViewModel
        {
            get { return (PixelChannelControlViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PixelChannelControlViewModel),
                typeof(PixelChannelControl), new PropertyMetadata(new PixelChannelControlViewModel(new())));

        public PixelChannelControl()
        {
            this.InitializeComponent();
        }
    }
}
