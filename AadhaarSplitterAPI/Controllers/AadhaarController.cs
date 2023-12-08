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

        public AadhaarController(IAadhaarService aadhaarService, ILogger<AadhaarController> logger)
        {
            _aadhaarService = aadhaarService ?? throw new ArgumentNullException(nameof(aadhaarService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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



