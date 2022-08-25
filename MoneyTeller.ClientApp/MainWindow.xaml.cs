﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        private void OnClickConvert(object sender, RoutedEventArgs e)
        {
            txtOutput.Clear();
            string amountAsText = txtInput.Text;
            if (decimal.TryParse(amountAsText,
                    _decimalParserStyle,
                    _decimalParserCulture,
                    out decimal amountAsNumber))
            {
                txtOutput.Text = $"Conversion successfull! {amountAsNumber}";
            }
            else
            {
                txtOutput.Text = "Could not parse provided amount!\nInput example: 12 345,67";
            }
        }
    }
}
