using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace SheepHucker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_COMMAND = 0x0112;      // Code for Windows command
        public const int WM_CLOSE = 0xF060;		 // Command code for close window

        public MainWindow()
        {
            InitializeComponent();

        }
    }
}
