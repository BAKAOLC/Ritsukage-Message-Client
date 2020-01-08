using System;
using System.Windows;
using System.Windows.Input;

namespace Ritsukage_Message_Client
{
    public class ReleaseFormHidden : ICommand
    {
        public void Execute(object parameter)
        {
            MainWindow.Form.Visibility = Visibility.Visible;
            MainWindow.Form.WindowState = WindowState.Normal;
        }

        public bool CanExecute(object parameter)
        {
            return (MainWindow.Form.Visibility == Visibility.Hidden);
        }

        public event EventHandler CanExecuteChanged;
    }
}