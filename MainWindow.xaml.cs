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
            // Format: HH:mm:ss.ff
            DaysText.Text = $"{diff.Hours:D2}:{diff.Minutes:D2}:{diff.Seconds:D2}.{diff.Milliseconds / 10:D2}";
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

            // Always Size to Content (Manual) to ensure Drag works properly
            DaysText.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            this.Width = DaysText.DesiredSize.Width + 60; // Padding
            this.Height = DaysText.DesiredSize.Height + 60;
        }

        public void GoFullScreen()
        {
            this.WindowState = WindowState.Normal;
            
            // Just ensure size is correct first
            ApplySettings(_config);
            
            // Center on Screen
            this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
            this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;
            
            // Styling
            DaysText.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            DaysText.VerticalAlignment = VerticalAlignment.Center;
            DaysText.Margin = new Thickness(0); 
        }

        public void GoMiniMode()
        {
            // Transition to Mini Mode
            ApplySettings(_config);

            // Top Left
            this.Left = 20;
            this.Top = 20;

            DaysText.Margin = new Thickness(10);
            DaysText.TextAlignment = TextAlignment.Center;
            DaysText.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
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