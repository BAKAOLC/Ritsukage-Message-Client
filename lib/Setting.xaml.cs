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
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Setting_1.IsChecked = MainWindow.user.AutoSign;
            Setting_2.IsChecked = MainWindow.user.DisablePopupMessage;
            Setting_3.IsChecked = MainWindow.user.UseEnterToSendMessage;
            Setting_4.Text = MainWindow.user.ReceiveMessageDelay.ToString();
            Setting_5.Text = MainWindow.user.UpdateCheckDelay.ToString();
        }

        private void DragMainForm(object sender, MouseButtonEventArgs e)
            => DragMove();

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
            => Close();

        private void Button_Minisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            MainWindow.Form.IsOpen_Setting = false;
        }

        private void SaveSetting_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.user.AutoSign = (bool)Setting_1.IsChecked;
            MainWindow.user.DisablePopupMessage = (bool)Setting_2.IsChecked;
            MainWindow.user.UseEnterToSendMessage = (bool)Setting_3.IsChecked;
            int delay;
            int.TryParse(Setting_4.Text, out delay);
            if (delay <= 0)
                delay = MainWindow.user.ReceiveMessageDelay;
            MainWindow.user.ReceiveMessageDelay = delay;
            int.TryParse(Setting_5.Text, out delay);
            if (delay <= 0)
                delay = MainWindow.user.UpdateCheckDelay;
            MainWindow.user.UpdateCheckDelay = delay;
            MainWindow.Form.SaveUserSetting();
            if (MainWindow.user.DisablePopupMessage)
                MainWindow.Form.popup.Visibility = Visibility.Hidden;
            else
                MainWindow.Form.popup.Visibility = Visibility.Visible;
            MainWindow.StopCheckVersion();
            MainWindow.StartCheckVersion();
            MainWindow.StopMessageReceive();
            MainWindow.StartMessageReceive();
            Close();
        }

        private Regex _NumberCheckRegex = new Regex("[^0-9]+");
        private void TextBox_Setting_4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting_4.Text = _NumberCheckRegex.Replace(Setting_4.Text, "");
        }
        private void TextBox_Setting_5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Setting_5.Text = _NumberCheckRegex.Replace(Setting_5.Text, "");
        }
    }
}
