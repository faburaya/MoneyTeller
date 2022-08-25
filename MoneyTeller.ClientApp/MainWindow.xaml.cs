using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private static readonly CultureInfo _decimalParserCulture;
        private static readonly NumberStyles _decimalParserStyle;

        static MainWindow()
        {
            _decimalParserCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            _decimalParserCulture.NumberFormat.NumberGroupSeparator = " ";
            _decimalParserCulture.NumberFormat.NumberDecimalSeparator = ",";
            _decimalParserStyle = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;
        }

        private static string MakeRequestUrlFor(decimal amount)
        {
            Uri prefix = new(
                ConfigurationManager.AppSettings["moneyTellerApiUrl"] ?? "API_URL_NOT_CONFIGURED");
            return new Uri(prefix, amount.ToString(CultureInfo.InvariantCulture)).ToString();
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
                txtOutput.Text = $"Sending HTTP request: {MakeRequestUrlFor(amountAsNumber)}\n";
                await Task.Run(() => Thread.Sleep(TimeSpan.FromMilliseconds(500)));
                txtOutput.Text += "\nDONE";
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
