using Microsoft.AspNetCore.Mvc;

namespace MoneyTeller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly ILogger<ConversionController> _logger;

        private readonly Conversion.IConverter _converter;

        public ConversionController(ILogger<ConversionController> logger)
        {
            _logger = logger;
            _converter = new Conversion.DollarConverter();
        }

        // GET api/<ConversionController>/123.45
        [HttpGet("{amount}")]
        public Serialization.ConversionResponse Get(decimal amount)
        {
            try
            {
                return new Serialization.ConversionResponse(_converter.ToWords(amount), null);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                _logger.LogError("Could not serve request: {error}", error);
                return new Serialization.ConversionResponse(null, $"Could not serve request: {ex.Message}");
            }
        }
    }
}
