using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ValueConverters
{
    public class EmailToInitialsValueConverter : BaseValueConverter<EmailToInitialsValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string initials = "";

            var email = (String)value;

            string[] words = email.Split(new char[] { '.' });


            foreach (string s in words)
            {
                if (words.Length > 1)
                {
                    initials += s.Remove(1);
                }
                else
                {
                    initials = s;
                }
            }
            return initials.Remove(initials.Length - 1).ToUpper();
            //return "Sosms";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
