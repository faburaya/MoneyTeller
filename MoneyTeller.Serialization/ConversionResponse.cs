using System.Text.Json.Serialization;

namespace MoneyTeller.Serialization
{
    /// <summary>
    /// Number-to-words conversion service response.
    /// </summary>
    /// <param name="AmountInWords">An amount of currency in words.</param>
    /// <param name="ErrorMessage">Available when a server side error occurs.</param>
    public readonly record struct ConversionResponse(
        [property: JsonPropertyName("amount")] string AmountInWords,
        [property: JsonPropertyName("error")] string? ErrorMessage
    );
}