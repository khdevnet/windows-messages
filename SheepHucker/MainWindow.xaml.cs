using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Common;
using Common.Converters;

namespace SheepHucker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr messageParam, IntPtr message);

        private void SendWindowsMessage(string applicationName, int messageCommand, IntPtr messageParam, IntPtr message)
        {
            Process[] applications = Process.GetProcessesByName(applicationName);
            if (applications.Length > 0)
            {
                var application = applications[0].MainWindowHandle;
                PostMessage(application, messageCommand, messageParam, message);
            }
        }

        private void MainWindow_OnMouseMoveMouseMove(object sender, MouseEventArgs e)
        {
            var relativePosition = e.GetPosition(this);
            SendWindowsMessage(
                SheepClientConstants.Name,
                WindowsMessageCode.MouseMove,
                IntPtr.Zero,
                new MouseCoordinatesConverter().Convert(new MousePoint((int)relativePosition.X, (int)relativePosition.Y)));
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            SendWindowsMessage(SheepClientConstants.Name, WindowsMessageCode.WindowClose, IntPtr.Zero, IntPtr.Zero);
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Right:
                    SendWindowsMessage(SheepClientConstants.Name, WindowsMessageCode.WindowClose, WindowsMessageParamCode.RightMouseClick, IntPtr.Zero);
                    Background = new ImageBrush(new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/content/winner.jpg")));
                    break;
                case MouseButton.Left:
                    SendWindowsMessage(SheepClientConstants.Name, WindowsMessageCode.MouseButtonDown, WindowsMessageParamCode.LeftMouseClick, IntPtr.Zero);
                    break;
            }
        }
    }
}