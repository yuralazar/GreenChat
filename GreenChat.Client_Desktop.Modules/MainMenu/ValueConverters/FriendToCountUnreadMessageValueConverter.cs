using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Data;
using GreenChat.Client_Desktop.Modules.Service.Handlers;
using GreenChat.Client_Desktop.Modules.Service.Models;
using Microsoft.Practices.Unity;

namespace GreenChat.Client_Desktop.Modules.MainMenu.ValueConverters
{
    public class FriendToCountUnreadMessageValueConverter : BaseValueConverter<FriendToCountUnreadMessageValueConverter>
    {
        private WebSocketsMessageHandler _handler;
        public FriendToCountUnreadMessageValueConverter()
        {
            UnityContainer unityContainer = (UnityContainer)Application.Current.Resources["IoC"];

            _handler = (WebSocketsMessageHandler)unityContainer.Resolve<WebSocketsMessageHandler>();
        }
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var friend = _handler._FriendsManager.GetById((string) value);
            //int count = _handler._PrivateMessagesManager.CountNewByOwner(friend);
            return "count";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}