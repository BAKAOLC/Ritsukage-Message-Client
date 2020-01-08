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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Ritsukage_Message_Client.lib
{
    /// <summary>
    /// MessageItem.xaml 的交互逻辑
    /// </summary>
    public partial class MessageItem : UserControl
    {
        Grid UpGrid;

        public string ContentMarkDown = "";
        public bool IsLink = false;

        public MessageItem(Grid upGrid, bool useOpacityEnter = false, bool noStartAnimation = false)
        {
            UseOpacityEnter = useOpacityEnter;
            NoStartAnimation = noStartAnimation;
            InitializeComponent();
            UpGrid = upGrid;
            UpGrid.Children.Add(this);
            FrameTimer.Tick += new EventHandler(Entering);
            FrameTimer.Tick += new EventHandler(Label_ExitLabel_OpacityDynamic);
            FrameTimer.Tick += new EventHandler(Label_ExtendInfo_ColorDynamic);
            FrameTimer.Tick += new EventHandler(CheckFloat);
            FrameTimer.Tick += new EventHandler(Exiting);
            FrameTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            FrameTimer.Start();
            SecondTimer.Tick += new EventHandler(CheckCloseTimer);
            SecondTimer.Interval = new TimeSpan(0, 0, 1);
            SecondTimer.Start();
        }

        private static DispatcherTimer FrameTimer = new DispatcherTimer();
        private static DispatcherTimer SecondTimer = new DispatcherTimer();

        private bool NoStartAnimation;
        private bool Starting;
        private double EnterWidth;
        private int EnterTimer = 0;
        private bool UseOpacityEnter;
        private void Enter()
        {
            if (!Starting && !NoStartAnimation)
            {
                Starting = true;
                EnterTimer = 30;
                if (UseOpacityEnter)
                    Opacity = 0;
                else
                {
                    EnterWidth = ActualWidth / EnterTimer;
                    Margin = new Thickness(Margin.Left + ActualWidth, Margin.Top, Margin.Right - ActualWidth, Margin.Bottom);
                }
            }
        }
        private void Entering(object sender, EventArgs e)
        {
            if (Starting && EnterTimer > 0)
            {
                if (UseOpacityEnter)
                    Opacity = 1 - (double)EnterTimer / 30;
                else
                    Margin = new Thickness(Margin.Left - EnterWidth, Margin.Top, Margin.Right + EnterWidth, Margin.Bottom);
                EnterTimer--;
            }
            else if (Starting)
            {
                if (UseOpacityEnter)
                    Opacity = 1;
                Starting = false;
                NoStartAnimation = true;
            }
        }

        private bool ExitLabelShow;
        private void Label_ExitLabel_OpacityDynamic(object sender, EventArgs e)
        {
            if (ExitLabelShow)
            {
                if (ExitLabel.Opacity >= 99.955)
                    ExitLabel.Opacity = 100;
                else
                    ExitLabel.Opacity += 0.045;
            }
            else
            {
                if (ExitLabel.Opacity <= 0.045)
                    ExitLabel.Opacity = 0;
                else
                    ExitLabel.Opacity -= 0.045;
            }
        }
        private void Label_ExitLabel_Hold(object sender, MouseEventArgs e)
        {
            ExitLabelShow = true;
        }
        private void Label_ExitLabel_Leave(object sender, MouseEventArgs e)
        {
            ExitLabelShow = false;
        }

        public bool Closed;
        private bool Closing;
        private void Exiting(object sender, EventArgs e)
        {
            if (Closing)
            {
                Margin = new Thickness(Margin.Left + 3, Margin.Top, Margin.Right - 3, Margin.Bottom);
                if (Opacity > 0.035)
                    Opacity -= 0.035;
                else
                {
                    Opacity = 0;
                    Closing = false;
                    Closed = true;
                    Visibility = Visibility.Hidden;
                    UpGrid.Children.Remove(this);
                }
            }
        }
        private void Button_ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Closing = true;
        }

        private void Enter(object sender, RoutedEventArgs e)
            => Enter();

        public int CloseTimer = -1;
        private void CheckCloseTimer(object sender, EventArgs e)
        {
            if (CloseTimer > 0)
                CloseTimer--;
            else if (CloseTimer == 0)
                Closing = true;
        }

        public int slot = 0;
        public double FloatSpeed = 5;
        public double FloatDistance = 0;
        private double _FloatDistance = 0;
        private void CheckFloat(object sender, EventArgs e)
        {
            double Float = FloatDistance - _FloatDistance;
            if (!Closed)
            {
                if (Float > 0)
                {
                    if (Float > FloatSpeed)
                    {
                        Margin = new Thickness(Margin.Left, Margin.Top - FloatSpeed, Margin.Right, Margin.Bottom + FloatSpeed);
                        _FloatDistance += FloatSpeed;
                    }
                    else if (Float != 0)
                    {
                        Margin = new Thickness(Margin.Left, Margin.Top - Float, Margin.Right, Margin.Bottom + Float);
                        _FloatDistance += Float;
                    }
                }
            }
        }

        bool ExtendInfoHolding;
        int ExtendInfoHoldingTrigger = 0;
        private void Label_ExtendInfo_ColorDynamic(object sender, EventArgs e)
        {
            if (ExtendInfoHolding)
                {
                if (ExtendInfoHoldingTrigger < 30)
                    ExtendInfoHoldingTrigger++;
                }
            else
                if (ExtendInfoHoldingTrigger > 0)
                    ExtendInfoHoldingTrigger--;
            ExtendInfoGrid.Background = new SolidColorBrush(Color.FromArgb(
                255,
                (byte)(24 + ExtendInfoHoldingTrigger * 3),
                (byte)(28 + ExtendInfoHoldingTrigger * 3),
                (byte)(24 + ExtendInfoHoldingTrigger * 3)));
        }
        private void Label_ExtendInfo_Hold(object sender, MouseEventArgs e)
        {
            ExtendInfoHolding = true;
        }
        private void Label_ExtendInfo_Leave(object sender, MouseEventArgs e)
        {
            ExtendInfoHolding = false;
        }
        private void Button_ExtendInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsLink)
                System.Diagnostics.Process.Start(ContentMarkDown);
            else
                new DetailedInformation((string)Title.Content, ContentMarkDown).Show();
        }
    }
}
