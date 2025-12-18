using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using Application = System.Windows.Application;

namespace DaysCounter
{
    public partial class App : Application
    {
        private NotifyIcon _notifyIcon;
        private MainWindow _mainWindow;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var config = AppConfig.Load();
            SetStartup(config.RunOnStartup);

            // Initialize MainWindow
            _mainWindow = new MainWindow();
            _mainWindow.Show();

            // Initialize Tray Icon
            _notifyIcon = new NotifyIcon();
            // Use a default system icon or create one. For now, system warning icon is a placeholder or just a simple generic one.
            // Ideally we load an .ico resource, but to keep it simple and almost no-asset:
            _notifyIcon.Icon = SystemIcons.Application; 
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "DaysCounter";
            
            // Context Menu
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Settings", null, (s, args) => OpenSettings());
            contextMenu.Items.Add("Exit", null, (s, args) => Shutdown());
            _notifyIcon.ContextMenuStrip = contextMenu;

            // Timer logic moved to MainWindow for better UI control
        }

        public void SetStartup(bool enable)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (enable)
                    {
                        string path = Environment.ProcessPath;
                        if (!string.IsNullOrEmpty(path))
                            key.SetValue("DaysCounter", path);
                    }
                    else
                    {
                        key.DeleteValue("DaysCounter", false);
                    }
                }
            }
            catch { }
        }

        private void OpenSettings()
        {
            var settings = new SettingsWindow();
            settings.ShowDialog();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
