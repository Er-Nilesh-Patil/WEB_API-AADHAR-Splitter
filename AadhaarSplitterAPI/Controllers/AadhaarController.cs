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

        
        public AadhaarController(IAdharService aadhaarService)
        {
            _aadhaarService = aadhaarService;
        }

        [HttpPost("FindAadhaar")]
        public ActionResult<List<string>> FindAadhaar([FromBody] string rawText)
        {
            try
            {
                
                List<string> aadhaarNumbers = _aadhaarService.ExtractAadhaarNumbers(rawText);

                
                return Ok(aadhaarNumbers);
            }
            catch (Exception ex)
            {
                
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
