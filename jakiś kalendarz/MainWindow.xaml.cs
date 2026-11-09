using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace jakiś_kalendarz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        private readonly HttpClient _httpClient = new();

        private const string PoznanUrl = "https://api.open-meteo.com/v1/forecast?latitude=52.4069&longitude=16.9299&current=temperature_2m";

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateClock();
        }

        private void UpdateClock()
        {
            DateTime now = DateTime.Now;

            TimeText.Text = now.ToString("HH:mm:ss");
            DateText.Text = now.ToString("dd.MM.yyyy");
        }
        public MainWindow()
        {
            InitializeComponent();

            UpdateClock();

            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Tick += Timer_Tick;
            _dispatcherTimer.Start();
        }

        private async Task LoadWeatherAsync(string requestUrl)
        {
            StatusText.Text = "Pobieram...";
            DownloadedText.Text = string.Empty;

            try
            {
                string json = await _httpClient.GetStringAsync(requestUrl);
                WeatherText.Text = json;
                DownloadedText.Text = $"Pobrano o {DateTime.Now:HH:mm:ss}";
            }
            catch (HttpRequestException)
            {
                WeatherText.Text = "Nie udało się pobrać danych pogodowych.";
            }
            finally
            {
                StatusText.Text = "Gotowe.";
            }
        }

        private async void PoznanButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadWeatherAsync(PoznanUrl); 
        }
    }
}
