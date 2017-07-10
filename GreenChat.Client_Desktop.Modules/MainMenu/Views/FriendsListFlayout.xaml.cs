using MahApps.Metro.Controls;
using PrismMahAppsSample.Infrastructure.Constants;
using PrismMahAppsSample.Infrastructure.Interfaces;

namespace GreenChat.Client_Desktop.Modules.MainMenu.Views
{
    /// <summary>
    /// Interaction logic for FriendsListFlayout
    /// </summary>
    public partial class FriendsListFlayout : Flyout, IFlyoutView
    {
        public FriendsListFlayout()
        {
            InitializeComponent();
        }

        public string FlyoutName
        {
            get
            {
                return FlyoutNames.FriendsListFlayout;
            }
        }
    }
}
