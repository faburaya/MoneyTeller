using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace MoneyTeller.Conversion
{
    /// <summary>
    /// Implements <see cref="IConverter"/> for Dollars.
    /// </summary>
    public class DollarConverter : IConverter
    {
        private static readonly string[] _spokenDigits =
            new[] { string.Empty,
                "one", "two", "three",
                "four", "five", "six",
                "seven", "eight", "nine" };

        private static void AppendDigit(uint number, StringBuilder buffer)
        {
            Debug.Assert(number < 10);
            buffer.Append(_spokenDigits[number]);
        }

        private static readonly string[] _spokenTys =
            new[] { string.Empty,
                string.Empty, "twenty", "thirty",
                "fourty", "fifty", "sixty",
                "seventy", "eighty", "ninety" };

        private static readonly string[] _spokenTeens =
            new[] { "ten",
                "eleven", "twelve", "thirteen",
                "fourteen", "fifteen", "sixteen",
                "seventeen", "eighteen", "nineteen" };

        private static void AppendTens(uint number, StringBuilder buffer)
        {
            Debug.Assert(number < 1e2);
            uint index = number / 10;
            uint lastDigit = number % 10;
            if (index == 1)
            {
                buffer.Append(_spokenTeens[lastDigit]);
                return;
            }

            buffer.Append(_spokenTys[index]);
            if (lastDigit != 0)
            {
                if (index >= 2)
                    buffer.Append('-');

                AppendDigit(lastDigit, buffer);
            }
        }

        private delegate void AppendPartCallback(uint number, StringBuilder buffer);

        private static void AppendPart(
            StringBuilder buffer,
            uint number,
            string suffix,
            uint minMultiple,
            uint upperLimit,
            AppendPartCallback nextHandler)
        {
            Debug.Assert(number < upperLimit);
            uint countParts = number / minMultiple;
            nextHandler(countParts, buffer);
            if (countParts > 0)
                buffer.Append($" {suffix}");

            uint remaider = number % minMultiple;
            if (remaider != 0)
            {
                if (countParts > 0)
                    buffer.Append(' ');

                nextHandler(remaider, buffer);
            }
        }

        private static void AppendHundreds(uint number, StringBuilder buffer)
        {
            AppendPart(buffer, number, "hundred", (uint)1e2, (uint)1e3, AppendTens);
        }

        private static void AppendThousands(uint number, StringBuilder buffer)
        {
            AppendPart(buffer, number, "thousand", (uint)1e3, (uint)1e6, AppendHundreds);
        }

        private static void AppendMillions(uint number, StringBuilder buffer)
        {
            AppendPart(buffer, number, "million", (uint)1e6, (uint)1e9, AppendThousands);
        }

        private static readonly decimal _one_cent;

        static DollarConverter()
        {
            CultureInfo culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            _one_cent = decimal.Parse("0.01", culture);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">If the input is out of range.</exception>
        public string ToWords(decimal amount)
        {
            if (amount < decimal.Zero)
            {
                throw new ArgumentOutOfRangeException($"Negative amount ({amount}) is not allowed!");
            }

            uint dollars = (uint)decimal.Floor(amount);
            const uint upperLimit = (uint)1e9;
            if (dollars >= upperLimit)
            {
                throw new ArgumentOutOfRangeException($"Cannot convert input greater than {upperLimit}!");
            }

            StringBuilder buffer = new();
            if (dollars > 0)
            {
                AppendMillions(dollars, buffer);
            }
            else
            {
                buffer.Append("zero");
            }

            buffer.Append(dollars != 1 ? " dollars" : " dollar");

            uint cents = (uint)((amount - dollars) / _one_cent);
            if (cents != 0)
            {
                buffer.Append(" and ");
                AppendTens(cents, buffer);
                buffer.Append(cents > 1 ? " cents" : " cent");
            }

            return buffer.ToString();
        }
    }
}