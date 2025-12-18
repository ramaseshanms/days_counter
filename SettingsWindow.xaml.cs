using System.Windows;
using System.Windows.Media;

namespace DaysCounter
{
    public partial class SettingsWindow : Window
    {
        private AppConfig _config;

        private bool _isLoaded = false;

        public SettingsWindow()
        {
            InitializeComponent();
            _config = AppConfig.Load();
            LoadSettings();
            _isLoaded = true;
        }

        private void LoadSettings()
        {
            ColorInput.Text = _config.TextColor;
            FontSlider.Value = _config.FontSize;
            ScaleSlider.Value = _config.OverlayScale;
            TargetDateInput.SelectedDate = _config.TargetDate;
            TimeDetailCheck.IsChecked = _config.ShowTimeDetail;
        }

        private void SettingChanged(object sender, RoutedEventArgs e)
        {
            if (!_isLoaded) return;

            UpdateConfigFromUI();
            
            // Notify MainWindow to update live
            if (System.Windows.Application.Current.MainWindow is MainWindow mw)
            {
                mw.ApplySettings(_config); // Pass transient config
            }
        }

        private void UpdateConfigFromUI()
        {
            _config.TextColor = ColorInput.Text;
            _config.FontSize = FontSlider.Value;
            _config.OverlayScale = ScaleSlider.Value;
            if (TargetDateInput.SelectedDate.HasValue)
                _config.TargetDate = TargetDateInput.SelectedDate.Value;
            _config.ShowTimeDetail = TimeDetailCheck.IsChecked ?? false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateConfigFromUI();
            _config.Save();
            Close();
        }
    }
}
