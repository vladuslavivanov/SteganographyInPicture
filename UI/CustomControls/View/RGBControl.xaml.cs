using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using SteganographyInPicture.UI.CustomControls.ViewModel;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteganographyInPicture.UI.CustomControls.View
{
    public sealed partial class RGBControl : UserControl
    {
        // Определение свойства зависимостей
        public RGBControlViewModel ViewModel
        {
            get { return (RGBControlViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(RGBControlViewModel), 
                typeof(RGBControl), new PropertyMetadata(new RGBControlViewModel(new(), 0)));

        public RGBControl()
        {
            this.InitializeComponent();
        }
    }
}
