using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ritsukage_Message_Client.lib
{
    /// <summary>
    /// UserAccount.xaml 的交互逻辑
    /// </summary>
    public partial class UserAccount : Window
    {
        public UserAccount()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            if (MainWindow.Form.hasLoadedUser)
            {
                InputQQ.Text = MainWindow.user.QQ.ToString();
                InputCode.Text = MainWindow.user.Code;
                AutoSign.IsChecked = MainWindow.user.AutoSign;
            }
        }

        private bool Connected;

        private void DragMainForm(object sender, MouseButtonEventArgs e)
            => DragMove();

        private void ReleaseTmpInfo(object sender, MouseEventArgs e)
           => MainWindow.Form.ReleaseTmpInfo(sender, e);
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Form.Close();
        }
        private void Button_Cancel_HoldInfo(object sender, MouseEventArgs e)
           => MainWindow.Form.SetTmpInfo("退出程序");

        private void Connect()
        {
            bool result = long.TryParse(InputQQ.Text, out long qq);
            if (result)
            {
                Connected = MainWindow.Form.Connect(qq, InputCode.Text.ToLower());
                if (!Connected)
                {
                    InputQQ.Text = "";
                    InputCode.Text = "";
                    ConnectInfo.Content = "未查找到用户信息\n可能QQ与Code并不匹配";
                }
                else
                    Close();
            }
            else
            {
                InputQQ.Text = "";
                ConnectInfo.Content = "错误的QQ号格式";
            }
        }
        private void Button_Confirm_Click(object sender, RoutedEventArgs e)
            => Connect();
        private void Button_Confirm_HoldInfo(object sender, MouseEventArgs e)
           => MainWindow.Form.SetTmpInfo("设置用户信息");

        private Regex _QQCheckRegex = new Regex("[^0-9]+");
        private void TextBox_InputQQ_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputQQ.Text = _QQCheckRegex.Replace(InputQQ.Text, "");
        }
        private void TextBox_InputQQ_HoldInfo(object sender, MouseEventArgs e)
           => MainWindow.Form.SetTmpInfo("用户QQ号");

        private Regex _CodeCheckRegex = new Regex("[^0-9a-zA-Z]+");
        private void TextBox_InputCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputCode.Text = _CodeCheckRegex.Replace(InputCode.Text, "");
        }
        private void TextBox_InputCode_HoldInfo(object sender, MouseEventArgs e)
           => MainWindow.Form.SetTmpInfo("对应QQ号获取的唯一Code");

        private void CheckBox_AutoSign_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.user.AutoSign = true;
        }

        private void CheckBox_AutoSign_UnChecked(object sender, RoutedEventArgs e)
        {
            MainWindow.user.AutoSign = false;
        }
        private void CheckBox_AutoSign_HoldInfo(object sender, MouseEventArgs e)
           => MainWindow.Form.SetTmpInfo("下次启动是否自动认证");

        private void OnCloseing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Connected)
                e.Cancel = true;
            DialogResult = Connected;
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            if ((bool)AutoSign.IsChecked) Connect();
        }
    }
}
