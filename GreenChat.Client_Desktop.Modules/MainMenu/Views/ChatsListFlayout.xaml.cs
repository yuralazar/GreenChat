using System.Windows;
using MahApps.Metro.Controls;
using PrismMahAppsSample.Infrastructure.Constants;
using PrismMahAppsSample.Infrastructure.Interfaces;

namespace GreenChat.Client_Desktop.Modules.MainMenu.Views
{
    /// <summary>
    /// Interaction logic for ChatsListFlayout
    /// </summary>
    public partial class ChatsListFlayout : Flyout, IFlyoutView
    {
        public ChatsListFlayout()
        {
            InitializeComponent();
        }

        public string FlyoutName
        {
            get
            {
                return FlyoutNames.ChatsListFlayout;
            }
        }

        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var m = new ModernDialog1();
        //    m.Show();
        //}
    }
}
