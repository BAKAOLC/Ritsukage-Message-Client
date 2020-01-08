using HeyRed.MarkdownSharp;
using HTMLConverter;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace Ritsukage_Message_Client.lib
{
    /// <summary>
    /// DetailedInformation.xaml 的交互逻辑
    /// </summary>
    public partial class DetailedInformation : Window
    {
        private string ContentHtml;
        private string ContentHtmlPath;
        public DetailedInformation(string title, string content)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            Title.Content = title;
            ContentHtml = GetHtmlFromMarkDown(content);
            ContentHtmlPath = Path.GetTempFileName();
            StreamWriter writer = new StreamWriter(ContentHtmlPath, false, Encoding.UTF8);
            writer.Write(ContentHtml);
            writer.Close();
            WebBrowser browser = new WebBrowser();
            browser.Navigate(ContentHtmlPath);
            ContentBrowserGrid.Children.Add(browser);
        }

        private string GetHtmlFromMarkDown(string markdown)
        {
            string md = new Markdown().Transform(markdown);
            string result = "<!DOCTYPE html><html lang=\"zh-CN\"><head><meta charset=\"UTF-8\"><meta http-equiv=\"content-type\" content=\"text/html;charset=utf-8\"></head><body>" + md + "</body></html>";
            return result;
        }

        private void DragMainForm(object sender, MouseButtonEventArgs e)
            => DragMove();

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
            => Close();

        private void Button_Minisize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.Delete(ContentHtmlPath);
        }
    }
}