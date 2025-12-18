using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace DaysCounter
{
    public partial class App : Application
    {
        private NotifyIcon _notifyIcon;
        private MainWindow _mainWindow;
        private System.Windows.Threading.DispatcherTimer _timer;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

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

            // Timer for 5 minutes
            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(5);
            _timer.Tick += (s, args) =>
            {
                _mainWindow.GoMiniMode();
                _timer.Stop();
            };
            _timer.Start();
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
