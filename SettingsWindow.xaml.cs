using System.Windows;
using System.Windows.Media;

namespace DaysCounter
{
    public partial class SettingsWindow : Window
    {
        private AppConfig _config;

        public SettingsWindow()
        {
            InitializeComponent();
            _config = AppConfig.Load();
            LoadSettings();
        }

        private void LoadSettings()
        {
            ColorInput.Text = _config.TextColor;
            FontSlider.Value = _config.FontSize;
            ScaleSlider.Value = _config.OverlayScale;
            TargetDateInput.SelectedDate = _config.TargetDate;
            TimeDetailCheck.IsChecked = _config.ShowTimeDetail;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _config.TextColor = ColorInput.Text;
            _config.FontSize = FontSlider.Value;
            _config.OverlayScale = ScaleSlider.Value;
            if (TargetDateInput.SelectedDate.HasValue)
                _config.TargetDate = TargetDateInput.SelectedDate.Value;
            _config.ShowTimeDetail = TimeDetailCheck.IsChecked ?? false;
            
            _config.Save();

            // Notify MainWindow to update if open
            if (System.Windows.Application.Current.MainWindow is MainWindow mw)
            {
                mw.ApplySettings();
            }

            Close();
        }
    }
}
