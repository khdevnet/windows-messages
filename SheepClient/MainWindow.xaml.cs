using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace SheepClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WM_MOUSEMOVE = 0x0200;

        public MainWindow()
        {
            InitializeComponent();
            var appSettings = ConfigurationManager.AppSettings;
            SheepPlayer.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + appSettings.Get("playerAvatar")));
            //this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/myapp;component/Images/icon.png")));
        }

        private void Window_OnLoaded(object sender, RoutedEventArgs e)
        {
            HwndSource.FromHwnd(new WindowInteropHelper(this).Handle)?.AddHook(this.WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_MOUSEMOVE:
                    int x = lParam.ToInt32() & 0x0000FFFF;
                    int y = (int)((lParam.ToInt32() & 0xFFFF0000) >> 16);
                    test.Content = x + " -" + y;
                    var newLeft = rnd.Next(Convert.ToInt32(cnv.ActualWidth - SheepPlayer.ActualWidth));
                    var newTop = rnd.Next(Convert.ToInt32(cnv.ActualHeight - SheepPlayer.ActualHeight));
                    MoveSheep(x, y);

                    // MouseMove?.Invoke();
                    break;
            }

            return IntPtr.Zero;
        }

        Random rnd = new Random();

        private void MoveSheep(int x, int y)
        {
            //Create the animations for left and top
            DoubleAnimation animLeft = new DoubleAnimation(Canvas.GetLeft(SheepPlayer), x, new Duration(TimeSpan.FromSeconds(1)));
            DoubleAnimation animTop = new DoubleAnimation(Canvas.GetTop(SheepPlayer), y, new Duration(TimeSpan.FromSeconds(1)));

            //Set an easing function so the button will quickly move away, then slow down
            //as it reaches its destination.
            animLeft.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            animTop.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };

            //Start the animation.
            SheepPlayer.BeginAnimation(Canvas.LeftProperty, animLeft, HandoffBehavior.SnapshotAndReplace);
            SheepPlayer.BeginAnimation(Canvas.TopProperty, animTop, HandoffBehavior.SnapshotAndReplace);
        }

        private void MainWindow_OnMouseMoveMouseMove(object sender, MouseEventArgs e)
        {
            var relativePosition = e.GetPosition(this);
            var point = PointToScreen(relativePosition);

            // _x.VerticalOffset = point.Y;

        }
    }
}
