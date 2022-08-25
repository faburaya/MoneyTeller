using System.Globalization;

using Xunit;

namespace MoneyTeller.Conversion.UnitTests
{
    public class DollarConverterTest
    {
        [Fact]
        public void ToWords_Zero()
        {
            DollarConverter converter = new();
            Assert.Equal("zero dollars", converter.ToWords(0));
        }

        [Fact]
        public void ToWords_One()
        {
            DollarConverter converter = new();
            Assert.Equal("one dollar", converter.ToWords(1));
        }

        private static readonly CultureInfo _decimalParserCulture;
        private static readonly NumberStyles _decimalParserStyle;

        static DollarConverterTest()
        {
            _decimalParserStyle = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;
            _decimalParserCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            _decimalParserCulture.NumberFormat.NumberGroupSeparator = ",";
            _decimalParserCulture.NumberFormat.NumberDecimalSeparator = ".";
        }

        private static void Test(string currency, string expected)
        {
            DollarConverter converter = new();
            decimal amount = decimal.Parse(currency, _decimalParserStyle, _decimalParserCulture);
            Assert.Equal(expected, converter.ToWords(amount));
        }

        [Theory]
        [InlineData("10", "ten dollars")]
        [InlineData("12", "twelve dollars")]
        [InlineData("14", "fourteen dollars")]
        [InlineData("32", "thirty-two dollars")]
        [InlineData("700", "seven hundred dollars")]
        [InlineData("801", "eight hundred one dollars")]
        [InlineData("918", "nine hundred eighteen dollars")]
        [InlineData("456", "four hundred fifty-six dollars")]
        [InlineData("8,043", "eight thousand fourty-three dollars")]
        [InlineData("7,654", "seven thousand six hundred fifty-four dollars")]
        [InlineData("20,987", "twenty thousand nine hundred eighty-seven dollars")]
        [InlineData("56,400", "fifty-six thousand four hundred dollars")]
        [InlineData("100,100", "one hundred thousand one hundred dollars")]
        [InlineData("203,500", "two hundred three thousand five hundred dollars")]
        [InlineData("654,320", "six hundred fifty-four thousand three hundred twenty dollars")]
        [InlineData("3,217,065", "three million two hundred seventeen thousand sixty-five dollars")]
        [InlineData("70,230,450", "seventy million two hundred thirty thousand four hundred fifty dollars")]
        [InlineData("876,040,030", "eight hundred seventy-six million fourty thousand thirty dollars")]
        public void ToWords_NoCents(string currency, string expected)
        {
            Test(currency, expected);
        }

        [Theory]
        [InlineData("0.01", "zero dollars and one cent")]
        [InlineData("0.02", "zero dollars and two cents")]
        [InlineData("0.10", "zero dollars and ten cents")]
        [InlineData("0.15", "zero dollars and fifteen cents")]
        [InlineData("0.30", "zero dollars and thirty cents")]
        [InlineData("0.57", "zero dollars and fifty-seven cents")]
        [InlineData("0.6", "zero dollars and sixty cents")]
        public void ToWords_OnlyCents(string currency, string expected)
        {
            Test(currency, expected);
        }

        [Fact]
        public void ToWords_WithCents()
        {
            Test("999,999,999.99",
                "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents");
        }

        [Fact]
        public void ToWords_UpperLimit_Throw()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new DollarConverter().ToWords((int)1e9));
        }

        [Fact]
        public void ToWords_Negative_Throw()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new DollarConverter().ToWords(-1));
        }
    }
}