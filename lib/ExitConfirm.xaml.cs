using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// ExitConfirmW.xaml 的交互逻辑
    /// </summary>
    public partial class ExitConfirm : Window
    {
        public ExitConfirm()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Topmost = true;
        }

        private bool Selected;

        private void DragMainForm(object sender, MouseButtonEventArgs e)
            => DragMove();

        private void ReleaseTmpInfo(object sender, MouseEventArgs e)
           => MainWindow.Form.ReleaseTmpInfo(sender, e);

        private void Button_No_Click(object sender, RoutedEventArgs e)
        {
            Selected = true;
            DialogResult = false;
        }
        private void Button_No_HoldInfo(object sender, MouseEventArgs e)
           => MainWindow.Form.SetTmpInfo("取消关闭操作");

        private void Button_Yes_Click(object sender, RoutedEventArgs e)
        {
            Selected = true;
            DialogResult = true;
        }
        private void Button_Yes_HoldInfo(object sender, MouseEventArgs e)
           => MainWindow.Form.SetTmpInfo("确认退出程序");

        private void OnCloseing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Selected)
                e.Cancel = true;
        }
    }
}
