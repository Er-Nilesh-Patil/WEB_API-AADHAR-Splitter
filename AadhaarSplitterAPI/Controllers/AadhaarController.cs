using AadhaarSplitterAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AadhaarSplitterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AadhaarController : ControllerBase
    {
        private readonly IAadhaarService _aadhaarService;
        private readonly ILogger<AadhaarController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AadhaarController"/> class.
        /// </summary>
        /// <param name="aadhaarService">The service for Aadhaar-related operations.</param>
        /// <param name="logger">The logger for logging.</param>
        public AadhaarController(IAadhaarService aadhaarService, ILogger<AadhaarController> logger)
        {
            _aadhaarService = aadhaarService ?? throw new ArgumentNullException(nameof(aadhaarService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// API endpoint for finding Aadhaar numbers in raw text.
        /// </summary>
        /// <param name="rawText">The raw text input.</param>
        /// <returns>
        /// Returns a list of valid Aadhaar numbers if successful; otherwise, returns a BadRequest response
        /// with an error message.
        /// </returns>
        [HttpPost("FindAadhaar")]
        public ActionResult<List<string>> FindAadhaar([FromBody, Required] string rawText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rawText))
                {
                    _logger.LogError("Raw text is empty or null.");
                    return BadRequest("Raw text cannot be empty or null.");
                }

                List<string> aadhaarNumbers = _aadhaarService.ExtractAadhaarNumbers(rawText);

                return Ok(aadhaarNumbers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing Aadhaar numbers. Raw Text: {RawText}", rawText);
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}



