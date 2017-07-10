using System.Windows;
using System.Windows.Controls;
using GreenChat.Client_Desktop.Modules.Authrorization.ViewModels;

namespace GreenChat.Client_Desktop.Modules.Authrorization.Views
{
    /// <summary>
    /// Interaction logic for RegistrationUserControl
    /// </summary>
    public partial class RegistrationUserControl : UserControl
    {
        public RegistrationUserControl()
        {
            InitializeComponent();
        }


        private void PasswordText_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((RegistrationUserControlViewModel)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }

        private void RepeatPassworText_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((RegistrationUserControlViewModel)this.DataContext).ConfirmPassword = ((PasswordBox)sender).Password; }
        }
    }
}
