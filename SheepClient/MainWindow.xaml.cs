using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Common;
using Common.Converters;

namespace SheepClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SheepPlayer.Source = GetPlayerAvatar();
        }

        private static BitmapImage GetPlayerAvatar()
        {
            var rnd = new Random();
            var id = rnd.Next(1, 4);
            return new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/content/sheepAvatar{id}.png"));
        }

        private void Window_OnLoaded(object sender, RoutedEventArgs e)
        {
            HwndSource.FromHwnd(new WindowInteropHelper(this).Handle)?.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int messageCode, IntPtr messageParam, IntPtr message, ref bool handled)
        {
            switch (messageCode)
            {
                case WindowsMessageCode.MouseMove:
                    var point = new MouseCoordinatesConverter().Convert(message);
                    MouseCoordinates.Content = point.X + " -" + point.Y;
                    MoveSheep(point.X, point.Y);
                    break;
                case WindowsMessageCode.WindowClose:
                    Close();
                    break;
                case WindowsMessageCode.MouseButtonDown:
                    if (messageParam == WindowsMessageParamCode.LeftMouseClick)
                    {
                        SheepPlayer.Source = GetPlayerAvatar();
                    }

                    break;
            }

            return IntPtr.Zero;
        }

        private void MoveSheep(int x, int y)
        {
            var animLeft = new DoubleAnimation(Canvas.GetLeft(SheepPlayer), x, new Duration(TimeSpan.FromSeconds(1)));
            var animTop = new DoubleAnimation(Canvas.GetTop(SheepPlayer), y, new Duration(TimeSpan.FromSeconds(1)));

            animLeft.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };
            animTop.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };

            SheepPlayer.BeginAnimation(Canvas.LeftProperty, animLeft, HandoffBehavior.SnapshotAndReplace);
            SheepPlayer.BeginAnimation(Canvas.TopProperty, animTop, HandoffBehavior.SnapshotAndReplace);
        }

        private void Close(CancelEventArgs e, bool cancel = false)
        {
            e.Cancel = cancel;
        }
    }
}