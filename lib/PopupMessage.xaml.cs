using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Ritsukage_Message_Client.lib
{
    /// <summary>
    /// PopupMessage.xaml 的交互逻辑
    /// </summary>
    public partial class PopupMessage : Window
    {
        public static PopupMessage Form;

        public PopupMessage()
        {
            InitializeComponent();
            Form = this;
            Topmost = true;
            Left = Screen.PrimaryScreen.Bounds.Width - Width;
            Top = Screen.PrimaryScreen.Bounds.Height - Height;
            Visibility = Visibility.Hidden;
            FrameTimer.Tick += new EventHandler(CheckPopup);
            FrameTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            FrameTimer.Start();
        }

        private static DispatcherTimer FrameTimer = new DispatcherTimer();

        private static ConcurrentDictionary<MessageItem, MessageItem> PopupMessageList = new ConcurrentDictionary<MessageItem, MessageItem>();
        private static ConcurrentDictionary<long, WaitPopueMessage> WaitingPopupMessageList = new ConcurrentDictionary<long, WaitPopueMessage>();

        private int PopupDelay = 3;
        private int PopupTimer = 0;
        private void CheckPopup(object sender, EventArgs e)
        {
            foreach (var pop in PopupMessageList)
            {
                if (pop.Value.Closed)
                {
                    PopupMessageList.TryRemove(pop.Key, out _);
                }
            }
            PopupTimer++;
            if (PopupTimer % PopupDelay == 0
                && WaitingPopupMessageList.Count > 0
                && PopupMessageList.Count <= 3)
            {
                foreach (var pop in PopupMessageList)
                {
                    pop.Value.slot++;
                }
                int n = 0;
                foreach (var pop in PopupMessageList.OrderBy(t => t.Value.slot))
                {
                    n++;
                    pop.Value.FloatDistance = 130 * n;
                }
                long msgId = 0;
                foreach (var pop in WaitingPopupMessageList.OrderBy(t => t.Key))
                {
                    msgId = pop.Key;
                    break;
                }
                WaitingPopupMessageList.TryRemove(msgId, out WaitPopueMessage item);
                MessageItem Msg = item.Popup;
                Msg.CloseTimer = 5;
                PopupMessageList.TryAdd(Msg, Msg);
            }

            if (PopupMessageList.Count <= 0)
                Visibility = Visibility.Hidden;
            else
                Visibility = Visibility.Visible;
        }

        class WaitPopueMessage
        {
            public Grid UpGrid { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string ContentMarkdown { get; set; }
            public bool IsLink { get; set; }
            public MessageItem Popup {
                get {
                    MessageItem item = new MessageItem(UpGrid);
                    item.Title.Content = Title;
                    item.Content.Text = Content;
                    item.ContentMarkDown = ContentMarkdown;
                    item.IsLink = IsLink;
                    if (IsLink)
                        item.ExtendInfo.Content = "> 点击此处唤起浏览器 <";
                    PopupMessageList.TryAdd(item, item);
                    return item;
                }
            }
        }

        long PopupMsgId = 0;
        public void PopupMsg(string title, string content, string markdown = null, bool isLink = false)
        {
            if (markdown == null)
                markdown = content;

            WaitPopueMessage item = new WaitPopueMessage();
            item.UpGrid = PopupMessageGrid;
            item.Title = title;
            if (content.Length > 60)
                content = content.Substring(0, 60) + "...";
            item.Content = content;
            item.ContentMarkdown = markdown;
            item.IsLink = isLink;
            WaitingPopupMessageList.TryAdd(PopupMsgId++, item);
        }
    }
}
