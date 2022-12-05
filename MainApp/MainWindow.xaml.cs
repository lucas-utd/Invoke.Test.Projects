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

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static NamedPipeServerStream NamedPipeServerStream { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private void  Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                NamedPipeServerStream = new("pipeStream1", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                NamedPipeServerStream.WaitForConnection();
            });
        }

        private void Invoke_Imaging_Click(object sender, RoutedEventArgs e)
        {
            string path = @"D:\Test\Invoke.Test.Projects\ImagingApp\bin\Debug\net6.0-windows\ImagingApp.exe";
            string[] arguments = Array.Empty<string>();

            // start
            ProcessStartInfo psi = new()
            {
                FileName = path,
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Minimized,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            foreach (var argument in arguments)
            {
                argument.Trim();
                psi.ArgumentList.Add(argument);
            }

            Process imagingApp = new();
            imagingApp.StartInfo = psi;
            imagingApp.OutputDataReceived += (sender, e) => Console.WriteLine("Imaging app: " + e.Data);
            imagingApp.ErrorDataReceived += (sender, e) => Console.WriteLine("Error in imaging app: " + e.Data);
            imagingApp.Start();
            imagingApp.BeginOutputReadLine();
            imagingApp.BeginErrorReadLine();
            imagingApp.WaitForExit();
        }

        private void Min_Imaging_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("The MainApp send minimize command.");
            if (NamedPipeServerStream.IsConnected)
            {
                Debug.WriteLine("The MainApp connect with ImagingApp, and send commands.");
                byte[] msg = Encoding.UTF8.GetBytes("minimize");
                NamedPipeServerStream.WriteAsync(msg, 0, msg.Length);
            }
        }

        private void Max_Imagin_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("The MainApp send maximize command.");
            if (NamedPipeServerStream.IsConnected)
            {
                Debug.WriteLine("The MainApp connect with ImagingApp, and send commands.");
                byte[] msg = Encoding.UTF8.GetBytes("maximize");
                NamedPipeServerStream.WriteAsync(msg, 0, msg.Length);
            }
        }
    }
}
