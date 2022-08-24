namespace MoneyTeller.Conversion
{
    /// <summary>
    /// Contract for conversion of amount of currenty into text.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Converts an amount of money into text.
        /// </summary>
        /// <param name="amount">An amount of money in some currency.</param>
        /// <returns>The given amount as text, spoken in English.</returns>
        string ToWords(decimal amount);
    }
}