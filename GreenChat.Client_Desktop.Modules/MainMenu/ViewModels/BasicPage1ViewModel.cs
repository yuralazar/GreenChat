using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using PrismMahAppsSample.Infrastructure.Base;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ViewModels
{
    public class BasicPage1ViewModel : ViewModelBase
    {
        public Visibility _isVisible = Visibility.Hidden;
        public Visibility IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        public BasicPage1ViewModel()
        {
            IsVisible = Visibility.Visible;
        }
    }
}
