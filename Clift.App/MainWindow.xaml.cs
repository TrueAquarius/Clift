using System;
using System.Collections.Generic;
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
using AirfoilView.Model;
using AirfoilView.Model.Airfoil;
using Microsoft.Win32;

namespace AirfoilView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NamedPipeServerStream? pipeServer;

        public MainWindow()
        {
            InitializeComponent();
            AllowDrop = true;
            
            string[] args = Environment.GetCommandLineArgs();
            // Check if any arguments are passed
            if (args.Length > 1)
            {
                // Process the command-line arguments
                for (int i = 1; i < args.Length; i++)
                {
                    string filePath = args[i];
                    string pwd = System.IO.Directory.GetCurrentDirectory();

                    AddFile(filePath);
                }
            }
            else
            {
                // No command-line arguments
            }

            StartPipeServer();

            // ... Rest of your application startup code ...
        }


        /// <summary>
        /// Handles files which are droppen onto the application window
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    AddFile(file);
                }
            }
        }


        /// <summary>
        /// Handles 'File->Open' in the application's menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_File_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XFoil (*.polar)|*.polar|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;

                AddFile(filename);
            }
        }


        /// <summary>
        /// Add a file to be displayed by the application
        /// </summary>
        /// <param name="filename"></param>
        public void AddFile(string filename)
        {
            Polars? p = Polars.LoadFromXfoilFile(filename);
            if (p == null)
            {
                // ToDo: Add error message here
                return;
            }

            Global.Instance.Polars.Add(p);
            Global.Instance.Visibility.Add(Visibility.Visible);

            this.Dispatcher.Invoke(() => {
                ChartArea.AddPolar();
            });
        }

        private void StartPipeServer()
        {
            pipeServer = new NamedPipeServerStream(Global.PipeName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            pipeServer.BeginWaitForConnection(HandleClientConnection, null);
        }

        private void HandleClientConnection(IAsyncResult result)
        {
            if (pipeServer == null) return;
            
            try
            {
                pipeServer.EndWaitForConnection(result);

                // Handle the received messages from the client
                //Task.Run(() => ReceiveMessages());
                ReceiveMessages();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
            finally
            {
                // Continue waiting for the next client connection
                if (!pipeServer.IsConnected)
                    StartPipeServer();
                //pipeServer.BeginWaitForConnection(HandleClientConnection, null);
            }
            
        }

        private void ReceiveMessages()
        {
            if(pipeServer == null) return;

            using (StreamReader reader = new StreamReader(pipeServer))
            {
                while (pipeServer.IsConnected)
                {
                    try
                    {
                        string? receivedMessage = reader.ReadLine();
                        if (!string.IsNullOrEmpty(receivedMessage))
                        {
                            // Perform your desired function based on the received message
                            PerformFunction(receivedMessage);
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Error #1: " + ex.Message); 
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error occurred while receiving message: " + ex.Message);
                    }
                }
            }
        }

        private void PerformFunction(string message)
        {
            // Perform your desired function here based on the received message
            Dispatcher.Invoke(() =>
            {
                AddFile(message);
            });
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            // Close the named pipe when the application is closing
            pipeServer?.Close();
            pipeServer?.Dispose();
        }
    }
}
