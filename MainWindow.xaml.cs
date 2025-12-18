using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DaysCounter
{
    public partial class MainWindow : Window
    {
        private AppConfig _config;

        private System.Windows.Threading.DispatcherTimer _updateTimer;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            
            _updateTimer = new System.Windows.Threading.DispatcherTimer();
            _updateTimer.Interval = TimeSpan.FromMilliseconds(100);
            _updateTimer.Tick += UpdateTimeText;
            
            ApplySettings();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Initial Fullscreen State
            GoFullScreen();
        }

        private void UpdateTimeText(object sender, EventArgs e)
        {
            if (_config == null || !_config.ShowTimeDetail) return;

            var diff = DateTime.Now - _config.TargetDate;
            // Format: X Days, HH:mm:ss.f
            DaysText.Text = $"{(int)diff.TotalDays} Days\n{diff.Hours:D2}:{diff.Minutes:D2}:{diff.Seconds:D2}.{diff.Milliseconds / 100}";
        }

        public void ApplySettings(AppConfig config = null)
        {
            _config = config ?? AppConfig.Load();
            
            try
            {
                DaysText.Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString(_config.TextColor);
            }
            catch { }

            DaysText.FontSize = _config.FontSize;
            
            if (_config.ShowTimeDetail)
            {
                _updateTimer.Start();
                UpdateTimeText(null, null); // Immediate update
            }
            else
            {
                _updateTimer.Stop();
                // Static Days Calculation
                var diff = (DateTime.Now.Date - _config.TargetDate.Date).Days;
                if (diff == 1) DaysText.Text = $"{diff} Day";
                else DaysText.Text = $"{diff} Days";
            }
            
            // Scale transform
            DaysText.LayoutTransform = new ScaleTransform(_config.OverlayScale, _config.OverlayScale);

            // Manual Sizing to prevent Jitter
            if (this.WindowState == WindowState.Normal && this.Width < SystemParameters.PrimaryScreenWidth)
            {
                DaysText.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                this.Width = DaysText.DesiredSize.Width + 60; // Padding
                this.Height = DaysText.DesiredSize.Height + 60;
            }
        }

        public void GoFullScreen()
        {
            this.WindowState = WindowState.Normal;
            
            // For fullscreen start, we can just use screen dimensions
            this.Left = 0;
            this.Top = 0;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            
            // Styling
            DaysText.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            DaysText.VerticalAlignment = VerticalAlignment.Center;
            DaysText.Margin = new Thickness(0); 
        }

        public void GoMiniMode()
        {
            // Transition to Mini Mode
            // Force an update to correct size
            ApplySettings(_config);

            // Top Left
            this.Left = 20;
            this.Top = 20;

            DaysText.Margin = new Thickness(10);
            DaysText.TextAlignment = TextAlignment.Center;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}