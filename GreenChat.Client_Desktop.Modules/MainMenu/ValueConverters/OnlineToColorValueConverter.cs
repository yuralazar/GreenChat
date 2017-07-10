using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ValueConverters
{
   public class OnlineToColorValueConverter : BaseValueConverter<OnlineToColorValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return (bool)value? new SolidColorBrush(Colors.LawnGreen): new SolidColorBrush(Colors.Red);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? new SolidColorBrush(Colors.LawnGreen) : new SolidColorBrush(Colors.Red);
        }
    }
}
