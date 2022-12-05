using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
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

namespace ImagingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static NamedPipeClientStream PipeClientStream { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                if (mePlayer.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //using NamedPipeClientStream namedPipeClientStream = new("pipeStream1");
            //namedPipeClientStream.Connect();
            //namedPipeClientStream.ReadMode = PipeTransmissionMode.Message;

            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //    {
            //        string message = ReadMessage(PipeClientStream);
            //        switch (message)
            //        {
            //            case "minimize":
            //                Debug.WriteLine("The ImageApp receive the minimize command.");
            //                App.Current.MainWindow.WindowState = WindowState.Minimized;
            //                break;
            //            case "maximize":
            //                Debug.WriteLine("The ImageApp receive the minimize command.");
            //                App.Current.MainWindow.WindowState = WindowState.Maximized;
            //                break;
            //        }
            //    }
            //});
        }

        #region Read Pipe Message

        public static string ReadMessage(PipeStream s)
        {
            MemoryStream ms = new();
            byte[] buffer = new byte[0x1000];      // Read in 4k byte blocks 

            do
            {
                ms.Write(buffer, 0, s.Read(buffer, 0, buffer.Length));
            }
            while (!s.IsMessageComplete);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        #endregion
    }
}
