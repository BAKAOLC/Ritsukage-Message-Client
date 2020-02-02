using Hardcodet.Wpf.TaskbarNotification;
using Newtonsoft.Json;
using Ritsukage_Message_Client.lib;
using Ritsukage_Message_Client.lib.MySQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Ritsukage_Message_Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly TaskbarIcon Taskbar = (TaskbarIcon)Application.Current.FindResource("Taskbar");

        public static MainWindow Form;

        public static long VERSIONID = 6;
        public static string VERSIONNAME = "0.0.6";
        public static long LATESTVERSIONID;
        public static string LATESTVERSIONNAME;

        public static UserSetting user = new UserSetting();

        private static readonly string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("\\", "/");
        private static readonly string configPath = appdataPath += "/";
        private static readonly string userSavePath = configPath + "RitsukageMessageClient_User.json";

        private static MessageReceive MessageReceiver;
        public static void StopMessageReceive() => MessageReceiver.Stop();
        public static void StartMessageReceive() => MessageReceiver.Start();

        public void SaveUserSetting()
        {
            try
            {
                string json = JsonConvert.SerializeObject(user);
                StreamWriter writer = new StreamWriter(userSavePath);
                writer.Write(json);
                writer.Close();
                writer.Dispose();
            }
            catch (Exception ex)
            {
                TipError("小律影", "保存配置信息时发生异常：" + ex.ToString());
            }
        }
        public bool hasLoadedUser;
        public void LoadUserSetting()
        {
            try
            {
                if (File.Exists(userSavePath))
                {
                    StreamReader reader = new StreamReader(userSavePath);
                    UserSetting obj = JsonConvert.DeserializeObject<UserSetting>(reader.ReadToEnd());
                    user.QQ = obj.QQ;
                    user.Code = obj.Code;
                    user.AutoSign = obj.AutoSign;
                    user.DisablePopupMessage = obj.DisablePopupMessage;
                    user.UseEnterToSendMessage = obj.UseEnterToSendMessage;
                    user.ReceiveMessageDelay = obj.ReceiveMessageDelay;
                    if (user.ReceiveMessageDelay <= 0)
                        user.ReceiveMessageDelay = 100;
                    user.UpdateCheckDelay = obj.UpdateCheckDelay;
                    if (user.UpdateCheckDelay <= 0)
                        user.UpdateCheckDelay = 1000;
                    hasLoadedUser = true;
                    reader.Close();
                    reader.Dispose();
                }
                else
                {
                    user.ReceiveMessageDelay = 100;
                    user.UpdateCheckDelay = 1000;
                }
            }
            catch (Exception ex)
            {
                TipError("小律影", "读取历史记录信息时发生异常：" + ex.ToString());
            }
        }

        private long GetMessageCount()
        {
            long result = 0;
            MySQLHelper sql = new MySQLHelper();
            try
            {
                sql.Connect();
                sql.DoSQLCommand("SELECT GetMessageCount()");
                var reader = sql.GetLastDataReader();
                if (reader == null)
                    throw new Exception("获取信息表数值失败");
                if (reader.Read())
                {
                    result = reader.GetInt64(0);
                }
                sql.Disconnect();
            }
            catch (Exception ex)
            {
                result = 0;
                sql.Disconnect();
                TipError("小律影", "获取版本信息时发生异常：" + ex.ToString());
            }
            return result;
        }

        private DispatcherTimer FrameTrigger;

        private bool isWorking;

        public PopupMessage popup = new PopupMessage();

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MySQLHelper.Set("botdatabase.ritsukage.com", 3306, "TaskGetter", "OSYTasksGetter", "ritsukagebot");
            InitializeComponent();
            Form = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            SetInfo("请认证用户信息");
            LoadUserSetting();
            if (!user.AutoSign)
                CheckVersion();
            bool result = (bool)new UserAccount().ShowDialog();
            if (result)
            {
                SaveUserSetting();
                SetInfo("");
                FrameTrigger = new DispatcherTimer();
                FrameTrigger.Tick += new EventHandler(ScrollMessageItemList);
                FrameTrigger.Interval = new TimeSpan(0, 0, 0, 0, 10);
                FrameTrigger.Start();
                popup.Show();
                popup.Owner = this;
                if (user.DisablePopupMessage)
                    popup.Visibility = Visibility.Hidden;
                MessageReceiver = MessageReceive.GetInstance(GetMessageCount());
                StartMessageReceive();
                StartCheckVersion();
                isWorking = true;
                Visibility = Visibility.Visible;
            }
        }

        private void DragMainForm(object sender, MouseButtonEventArgs e)
            => DragMove();

        private string lastInfo;
        public void SetInfo(string info)
        {
            lastInfo = info;
            Info.Content = info;
        }
        public void SetTmpInfo(string info)
        {
            Info.Content = info;
        }
        public void ReleaseTmpInfo(object sender, MouseEventArgs e)
        {
            Info.Content = lastInfo;
        }

        public void TipNone(string title, string msg)
        {
            Taskbar.ShowBalloonTip(title, msg, BalloonIcon.None);
        }
        public void TipInfo(string title, string msg)
        {
            Taskbar.ShowBalloonTip(title, msg, BalloonIcon.Info);
        }
        public void TipWarning(string title, string msg)
        {
            Taskbar.ShowBalloonTip(title, msg, BalloonIcon.Warning);
        }
        public void TipError(string title, string msg)
        {
            Taskbar.ShowBalloonTip(title, msg, BalloonIcon.Error);
        }

        private bool MinisizeTip;
        private void OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState != WindowState.Normal)
            {
                if (WindowState == WindowState.Minimized)
                {
                    Visibility = Visibility.Hidden;
                    if (!MinisizeTip)
                    {
                        TipInfo("小律影", "可以点击任务栏图标恢复窗口哦~！");
                        MinisizeTip = true;
                    }
                }
                WindowState = WindowState.Normal;
            }
        }

        public static bool StopApplication;
        private void OnCloseing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool result = (bool)new ExitConfirm().ShowDialog();
            if (result)
            {
                StopApplication = true;
                Application.Current.Shutdown();
            }
            else
                e.Cancel = true;
        }
        
        private long versionUpdateTip = -1;
        private static readonly ThreadStart VersionChecker = new ThreadStart(VersionCheckerThreadCallback);
        private static Thread VersionCheckerThread;
        private static bool MissConnect = false;
        private static bool isCheckingVersion = false;
        private void CheckVersion()
        {
            MySQLHelper sql = new MySQLHelper();
            try
            {
                sql.Connect();
                sql.DoSQLCommand("SELECT * FROM version ORDER BY id DESC LIMIT 1;");
                var reader = sql.GetLastDataReader();
                if (reader == null)
                    throw new Exception();
                if (reader.Read())
                {
                    LATESTVERSIONID = reader.GetInt64(0);
                    LATESTVERSIONNAME = reader.GetString(1);
                    if (LATESTVERSIONID > VERSIONID)
                    {
                        if (versionUpdateTip < LATESTVERSIONID)
                        {
                            if (isWorking)
                            {
                                versionUpdateTip = LATESTVERSIONID;
                                TipInfo("小律影", string.Format(
                                    "检查到版本更新：\n当前版本号：{0}\n最新版本号：{1}\n更新时间：{2}\n",
                                    VERSIONNAME, LATESTVERSIONNAME, reader.GetDateTime(2).ToString()));
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    NewMessage("检测到新版本更新", string.Format(
                                       "当前版本号：{0}\n最新版本号：{1}\n更新时间：{2}\n",
                                       VERSIONNAME, LATESTVERSIONNAME, reader.GetDateTime(2).ToString()));
                                });
                                
                            }
                            else
                            {
                                TipInfo("小律影", string.Format(
                                    "检查到版本更新：\n当前版本号：{0}\n最新版本号：{1}\n更新时间：{2}\n",
                                    VERSIONNAME, LATESTVERSIONNAME, reader.GetDateTime(2).ToString()));
                            }
                        }
                    }
                }
                sql.Disconnect();
                MissConnect = false;
            }
            catch
            {
                sql.Disconnect();
                if (!MissConnect)
                    TipError("小律影", "版本更新信息获取失败");
                MissConnect = true;
            }
        }
        private static void VersionCheckerThreadCallback()
        {
            while (!StopApplication)
            {
                Thread.Sleep(user.UpdateCheckDelay);
                Form.CheckVersion();
            }
        }
        public static void StartCheckVersion()
        {
            if (!isCheckingVersion)
            {
                isCheckingVersion = true;
                VersionCheckerThread = new Thread(VersionChecker);
                VersionCheckerThread.Start();
            }
        }
        public static void StopCheckVersion()
        {
            if (isCheckingVersion)
            {
                VersionCheckerThread.Abort();
                VersionCheckerThread = null;
                isCheckingVersion = false;
            }
        }

        public bool Connect(long qq, string code)
        {
            bool result = false;
            MySQLHelper sql = new MySQLHelper();
            try
            {
                sql.Connect();
                sql.DoSQLCommand(string.Format("SELECT FindQQ({0},\"{1}\")", qq, code));
                var reader = sql.GetLastDataReader();
                if (reader == null)
                    throw new Exception();
                if (reader.Read())
                {
                    result = (reader.GetByte(0) == 1);
                    if (result)
                    {
                        user.QQ = qq;
                        user.Code = code;
                        TipInfo("小律影", "用户信息认证成功");
                    }
                }
                sql.Disconnect();
            }
            catch
            {
                sql.Disconnect();
                TipError("小律影", "查询用户信息失败");
            }
            return result;
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
            => Close();
        private void Button_Exit_HoldInfo(object sender, MouseEventArgs e)
            => SetTmpInfo("关闭小律影消息客户端");

        private void Button_Minisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Button_Minisize_HoldInfo(object sender, MouseEventArgs e)
            => SetTmpInfo("最小化小律影消息客户端");

        private bool AutoScroll = true;
        private void CheckBox_AutoScrollSetter_Checked(object sender, RoutedEventArgs e)
        {
            AutoScroll = true;
        }
        private void CheckBox_AutoScrollSetter_UnChecked(object sender, RoutedEventArgs e)
        {
            AutoScroll = false;
        }
        private void CheckBox_AutoScrollSetter_HoldInfo(object sender, MouseEventArgs e)
            => SetTmpInfo("设置列表自动滚动");

        public Grid InsertMessageItem(string title, string content, string markdown = null, bool isLink = false)
        {
            if (markdown == null)
                markdown = content;

            Grid ItemGrid = new Grid();
            ItemGrid.Loaded += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                if (AutoScroll)
                {
                    double pos = MessageItemListViewer.ScrollableHeight;
                    if (ItemGrid.ActualHeight > MessageItemListViewer.ViewportHeight)
                        pos = pos - ItemGrid.ActualHeight + MessageItemListViewer.ViewportHeight;
                    ScrollMessageItemListTo(pos);
                }
            });
            MessageItemList.Children.Add(ItemGrid);
            MessageItem item = new MessageItem(ItemGrid, true);
            item.Title.Content = title;
            item.Content.Text = content;
            item.ContentMarkDown = markdown;
            item.IsLink = isLink;
            if (item.IsLink)
                item.ExtendInfo.Content = "> 点击此处唤起浏览器 <";
            item.ExitButton.Visibility = Visibility.Hidden;
            return ItemGrid;
        }

        public void NewMessage(string title, string content, string markdown = null, bool isLink = false)
        {
            if (markdown == null)
                markdown = content;

            PopupMessage.Form.PopupMsg(title, content, markdown, isLink);
            InsertMessageItem(title, content, markdown, isLink);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
            => NewMessage(TitleBox.Text, ContentBox.Text);

        private bool Scrolling;
        private double TargetScrollOffset = 0;
        private double ScrollSpeed = 0.1;
        private double MinScrollOffset = 0.05;
        public void ScrollMessageItemListTo(double targetOffset)
        {
            TargetScrollOffset = targetOffset;
            Scrolling = true;
        }

        private void ScrollMessageItemList(object sender, EventArgs e)
        {
            if (Scrolling)
            {
                double offset = TargetScrollOffset - MessageItemListViewer.VerticalOffset;
                if (offset != 0)
                {
                    offset *= ScrollSpeed;
                    if (offset < MinScrollOffset && offset > -MinScrollOffset)
                        MessageItemListViewer.ScrollToVerticalOffset(TargetScrollOffset);
                    else
                        MessageItemListViewer.ScrollToVerticalOffset(MessageItemListViewer.VerticalOffset + offset);
                }
                else
                    Scrolling = false;
            }
        }
        private void ScrollViewer_MessageItemListViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Scrolling = false;
        }
        private void ScrollViewer_MessageItemListViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Scrolling = false;
        }

        private int charLimit = 2000;

        public bool SendMessage(long qq, string code, string msg)
        {
            bool result = false;
            MySQLHelper sql = new MySQLHelper();
            try
            {
                sql.Connect();
                sql.DoSQLCommand(string.Format("SELECT SendMessage({0}, \"{1}\", \"{2}\")", qq, code, msg.Replace("\"", "\\\"")));
                var reader = sql.GetLastDataReader();
                if (reader == null)
                    throw new Exception();
                if (reader.Read())
                {
                    result = (reader.GetByte(0) == 1);
                    if (!result)
                        throw new Exception();
                }
                sql.Disconnect();
                TipInfo("小律影", "消息发送成功");
            }
            catch
            {
                sql.Disconnect();
                TipError("小律影", "消息发送失败");
            }
            return result;
        }

        private void SendClientMessageBoxText()
        {
            if (_ClientMessageCheckRegex.IsMatch(ClientMessage.Text))
            {
                if (ClientMessage.Text.Length > 2000)
                    TipError("小律影", "语句太长啦！");
                else
                {
                    SendMessage(user.QQ, user.Code, ClientMessage.Text);
                    ClientMessage.Text = "";
                }
            }
        }

        private Regex _ClientMessageCheckRegex = new Regex(@"\S");
        private void Button_SendMessage_Click(object sender, RoutedEventArgs e)
            => SendClientMessageBoxText();
        private void Button_SendMessage_HoldInfo(object sender, MouseEventArgs e)
            => SetTmpInfo("发送消息");

        private void TextBox_ClientMessage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int index = ClientMessage.CaretIndex;
            if (user.UseEnterToSendMessage)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.None && e.Key == Key.Enter)
                {
                    SendClientMessageBoxText();
                    e.Handled = true;
                }
                else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
                {
                    ClientMessage.Text = ClientMessage.Text.Substring(0, index) + "\n" + ClientMessage.Text.Substring(index);
                    ClientMessage.CaretIndex = index + 1;
                }
            }
            else
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
                    SendClientMessageBoxText();
            }
        }
        private void TextBox_ClientMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            int charLeft = charLimit - ClientMessage.Text.Length;
            ClientMessageCharLeft.Content = charLeft;
            if (charLeft < 0)
                ClientMessageCharLeft.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else
                ClientMessageCharLeft.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }

        private void Button_Setting_HoldInfo(object sender, MouseEventArgs e)
            => SetTmpInfo("打开设置界面");
        public bool IsOpen_Setting;
        private void Button_Setting_Click(object sender, RoutedEventArgs e)
        {
            if (!IsOpen_Setting)
            {
                IsOpen_Setting = true;
                new Setting().Show();
            }
        }
    }

    public class UserSetting
    {
        public long QQ { get; set; }
        public string Code { get; set; }
        public bool AutoSign { get; set; }
        public bool DisablePopupMessage { get; set; }
        public bool UseEnterToSendMessage { get; set; }
        public int ReceiveMessageDelay { get; set; }
        public int UpdateCheckDelay { get; set; }
    }
}