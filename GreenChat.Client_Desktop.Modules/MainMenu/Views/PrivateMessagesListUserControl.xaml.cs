using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GreenChat.Client_Desktop.Modules.Service;
using GreenChat.Client_Desktop.Modules.Service.Handlers;

namespace GreenChat.Client_Desktop.Modules.MainMenu.Views
{
    /// <summary>
    /// Interaction logic for PrivateMessagesListUserControl
    /// </summary>
    public partial class PrivateMessagesListUserControl : UserControl
    {
        private readonly WebSocketsMessageSender _webSocketsMessageSender;
        private readonly ChatGlobals _chatGlobals;

        public PrivateMessagesListUserControl(WebSocketsMessageSender webSocketsMessageSender,
                                              ChatGlobals chatGlobals )
        {
            _webSocketsMessageSender = webSocketsMessageSender;
            _chatGlobals = chatGlobals;
            InitializeComponent();
        }

        private async void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer) sender;
            if (Math.Abs(scrollViewer.VerticalOffset) < 0.01)
            {       
                if (_chatGlobals.CurrentChat == null)
                    await _webSocketsMessageSender.SendMessageLoadCurrentFriendMessages(true);
                else
                    await _webSocketsMessageSender.SendMessageLoadCurrentChatMessages(true);
                scrollViewer.ScrollToVerticalOffset(1);
            }                
        }
    }
}
