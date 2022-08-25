using System;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyTeller.ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static readonly HttpClient _httpClient;
        private static readonly CultureInfo _decimalParserCulture;
        private static readonly NumberStyles _decimalParserStyle;

        static MainWindow()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MoneyTeller Client Application");

            _decimalParserCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            _decimalParserCulture.NumberFormat.NumberGroupSeparator = " ";
            _decimalParserCulture.NumberFormat.NumberDecimalSeparator = ",";
            _decimalParserStyle = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;
        }

        private static string MakeRequestUrlFor(decimal amount)
        {
            StringBuilder builder = new();
            builder.Append(ConfigurationManager.AppSettings["moneyTellerApiUrl"] ?? "API_URL_NOT_SET");
            if (builder[^1] != '/')
            {
                builder.Append('/');
            }
            builder.Append(amount.ToString(CultureInfo.InvariantCulture));
            return builder.ToString();
        }

        private async Task<Serialization.ConversionResponse> RequestConversion(decimal amount)
        {
            string url = MakeRequestUrlFor(amount);
            txtOutput.Text = $"Sending HTTP request: {url}\n";
            string payload = await _httpClient.GetStringAsync(url);
            return JsonSerializer.Deserialize<Serialization.ConversionResponse>(payload);
        }

        private async void OnClickConvert(object sender, RoutedEventArgs e)
        {
            txtOutput.Clear();
            string amountAsText = txtInput.Text;
            if (decimal.TryParse(amountAsText,
                    _decimalParserStyle,
                    _decimalParserCulture,
                    out decimal amountAsNumber))
            {
                btnConvert.IsEnabled = false;
                txtInput.IsEnabled = false;
                Serialization.ConversionResponse response = await RequestConversion(amountAsNumber);
                txtOutput.Text += $"\nResponse:\n{response.AmountInWords ?? response.ErrorMessage}";
                btnConvert.IsEnabled = true;
                txtInput.IsEnabled = true;
            }
            else
            {
                txtOutput.Text = "Could not parse provided amount!\nInput example: 12 345,67";
            }
        }
    }
}
