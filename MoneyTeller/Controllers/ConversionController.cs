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
        public ConversionResponse Get(decimal amount)
        {
            try
            {
                return new ConversionResponse(_converter.ToWords(amount));
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not process request: {ex}", ex);
                return new ConversionResponse("");
            }
        }
    }
}
