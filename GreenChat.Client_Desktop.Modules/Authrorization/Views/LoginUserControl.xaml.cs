using System.Windows;
using System.Windows.Controls;
using GreenChat.Client_Desktop.Modules.Authrorization.ViewModels;

namespace GreenChat.Client_Desktop.Modules.Authrorization.Views
{
    /// <summary>
    /// Interaction logic for LoginUserControl
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public LoginUserControl()
        {
            InitializeComponent();
        }

        private void LoginPassworText_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((LoginUserControlViewModel)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
}
