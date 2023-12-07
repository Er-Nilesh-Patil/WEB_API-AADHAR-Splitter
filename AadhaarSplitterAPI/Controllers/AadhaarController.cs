using AadhaarSplitterAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AadhaarSplitterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AadhaarController : ControllerBase
    {
        private readonly IAdharService _aadhaarService;

        // Constructor injection to initialize the service
        public AadhaarController(IAdharService aadhaarService)
        {
            _aadhaarService = aadhaarService;
        }

        [HttpPost("FindAadhaar")]
        public ActionResult<List<string>> FindAadhaar([FromBody] string rawText)
        {
            try
            {
                // Implement the FindAadhaar function logic here
                List<string> aadhaarNumbers = _aadhaarService.ExtractAadhaarNumbers(rawText);

                // Return a list of Aadhaar numbers with a 200 (OK) status
                return Ok(aadhaarNumbers);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately and return a 400 (Bad Request) status
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
