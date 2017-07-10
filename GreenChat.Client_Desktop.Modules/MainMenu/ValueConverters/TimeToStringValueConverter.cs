using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ValueConverters
{
    public class TimeToStringValueConverter : BaseValueConverter<TimeToStringValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (DateTimeOffset) value;

            if (time.Date == DateTimeOffset.UtcNow.Date)
            {
                return time.ToLocalTime().ToString("HH:mm");
            }
            return time.ToLocalTime().ToString("HH:mm, dd MMM yyyy");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
