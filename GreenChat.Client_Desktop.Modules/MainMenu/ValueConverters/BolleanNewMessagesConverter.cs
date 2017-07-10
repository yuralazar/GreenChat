using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ValueConverters
{
    class BolleanNewMessagesConverter : BaseValueConverter<BolleanNewMessagesConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string) value == String.Empty) { 
                return Visibility.Hidden;
            }
            else return  Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
