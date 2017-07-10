using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ValueConverters
{
    public class SendByMeBackgroundValueConverter : BaseValueConverter<SendByMeBackgroundValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value
                ? Application.Current.FindResource("MessageBubleBackgroundBrush")
                : Application.Current.FindResource("MyMessageBubleBackgroundBrush");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
