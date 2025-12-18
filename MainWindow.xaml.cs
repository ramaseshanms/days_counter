using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DaysCounter
{
    public partial class MainWindow : Window
    {
        private AppConfig _config;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            ApplySettings();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Initial Fullscreen State
            GoFullScreen();
        }

        public void ApplySettings()
        {
            _config = AppConfig.Load();
            
            try
            {
                DaysText.Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString(_config.TextColor);
            }
            catch { }

            DaysText.FontSize = _config.FontSize;
            
            var diff = DateTime.Now - _config.TargetDate;

            if (_config.ShowTimeDetail)
            {
                // Format: "X Days, HH:mm:ss"
                DaysText.Text = $"{(int)diff.TotalDays} Days\n{diff.Hours:D2}:{diff.Minutes:D2}:{diff.Seconds:D2}";
            }
            else
            {
                // Calculate days
                var days = (int)diff.TotalDays;
                
                // Handle wording
                if (days == 1) DaysText.Text = $"{days} Day";
                else DaysText.Text = $"{days} Days";
            }
            
            // Scale transform
            DaysText.LayoutTransform = new ScaleTransform(_config.OverlayScale, _config.OverlayScale);
        }

        public void GoFullScreen()
        {
            this.WindowState = WindowState.Normal;
            this.Left = 0;
            this.Top = 0;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            
            // Center text
            DaysText.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            DaysText.VerticalAlignment = VerticalAlignment.Center;
            DaysText.Margin = new Thickness(0);
        }

        public void GoMiniMode()
        {
            this.Width = 350 * _config.OverlayScale; // Slightly wider for time
            this.Height = 150 * _config.OverlayScale;
            
            // Top Left
            this.Left = 20;
            this.Top = 20;

            // Align text to center of mini box (looks better draggable)
            DaysText.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            DaysText.VerticalAlignment = VerticalAlignment.Center;
            DaysText.Margin = new Thickness(0);
            DaysText.TextAlignment = TextAlignment.Center;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }
    }
}