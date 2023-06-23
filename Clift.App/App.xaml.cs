using AirfoilView.Model;
using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.IO.Pipes;

namespace AirfoilView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Create an instance of the singleton 'Global' and keep it alive
        private Global global = Global.Instance;
        private static Mutex? mutex;
        

        public App()
        {
            Global.Instance.Preferences.Reset();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Create a unique identifier for the application instance
            string appId = Assembly.GetExecutingAssembly().GetType().GUID.ToString();

            // Create a named mutex using the unique identifier
            mutex = new Mutex(true, appId, out bool isNewInstance);

            if (isNewInstance)
            {
                // If this is a new instance, continue with application startup
                base.OnStartup(e);
            }
            else
            {
                // If this is not a new instance, activate the existing instance
                Process currentProcess = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);

                foreach (Process process in processes)
                {
                    if (process.Id != currentProcess.Id)
                    {
                        NativeMethods.SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                SendCommandLineArgumentsToExistingInstance(Environment.GetCommandLineArgs());

                // Exit the current instance
                Environment.Exit(0);
            }
        }

        private static void SendCommandLineArgumentsToExistingInstance(string[] args)
        {
            if (args.Length >= 1)
            {
                try
                {
                    using (var pipeClient = new NamedPipeClientStream(".", Global.PipeName, PipeDirection.Out))
                    {
                        pipeClient.Connect(5000); // Timeout in milliseconds

                        using (var sw = new StreamWriter(pipeClient))
                        {
                            for(int i=1; i<args.Length; ++i)
                            {
                                sw.WriteLine(args[i]);
                            }
                        }
                    }
                }
                catch (TimeoutException)
                {
                    // Handle timeout exception (existing instance not found or unable to connect)
                }
                catch (Exception)
                {
                    // Handle any other exceptions
                }
            }
        }
    }



    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [System.Security.SuppressUnmanagedCodeSecurity]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}

