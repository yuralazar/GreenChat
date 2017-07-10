using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GreenChat.Client_Desktop.Modules.MainMenu.Views
{
    /// <summary>
    /// Interaction logic for TestModernDialog.xaml
    /// </summary>
    public partial class TestModernDialog : ModernDialog
    {
        public TestModernDialog()
        {
            InitializeComponent();

            // define the dialog buttons
            ModernButton m = new ModernButton()
            {
                Height = 10,
                Width = 7,
                EllipseDiameter = 2.1,
                Name = "Okay",
                Background = new BitmapCacheBrush(),
                Padding = new Thickness(2, 2, 2, 3)
            };

            this.Buttons = new Button[] { /*this.OkButton, this.CancelButton,*/ m };
        }
    }
}
