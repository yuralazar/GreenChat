using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ValueConverters
{
    public class EmailToAvatarColorValueConverter : BaseValueConverter<EmailToAvatarColorValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (String)value;
            string strcolor = "";

            Dictionary<string, string> colors = new Dictionary<string, string>
            {
                ["abcdef"] = "#3099c5", //Blue 1 
                ["ghijkl"] = "#8000FF", //Yellow 7
                ["mnopqr"] = "#ffa800", //Orange 3
                ["stuvwx"] = "#ff4747", //Red 4 
                ["yz1234"] = "#00c541", //Green 5 
                ["567890"] = "#8000FF",
                [""] = "8000FF"//Purple 6 
            };

            foreach (KeyValuePair<string, string> keyValue in colors)
            {
                if (keyValue.Key.Contains(str.ToLower()[0]))
                {
                    strcolor = keyValue.Value;
                }
            }
            return strcolor != String.Empty ? (SolidColorBrush)(new BrushConverter().ConvertFrom($"{strcolor}")) : new SolidColorBrush(Colors.Black);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
