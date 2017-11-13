using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace SheepHucker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public const int WM_COMMAND = 0x0112;      // Code for Windows command
        public const int WM_CLOSE = 0xF060;		 // Command code for close window
        public const int WM_MOUSEMOVE = 0x0200;

        public MainWindow()
        {
            InitializeComponent();


        }

        private void MainWindow_OnMouseMoveMouseMove(object sender, MouseEventArgs e)
        {
            var relativePosition = e.GetPosition(this);
            var point = PointToScreen(relativePosition); 

            SendMousePosition("SheepClient", WM_MOUSEMOVE, (IntPtr)0, MakeLParam((int)relativePosition.X, (int)relativePosition.Y));
            // test.Content = point.X + " - " + point.Y;
        }

        public static IntPtr MakeLParam(int wLow, int wHigh)
        {
            return (IntPtr)(((short)wHigh << 16) | (wLow & 0xffff));
        }

        private void SendMousePosition(string hWnd, int Msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                IntPtr hWndNotepad = Process.GetProcessesByName(hWnd)[0].MainWindowHandle;
                if (hWndNotepad != null)
                {
                    PostMessage(hWndNotepad, Msg, wParam, lParam);
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
