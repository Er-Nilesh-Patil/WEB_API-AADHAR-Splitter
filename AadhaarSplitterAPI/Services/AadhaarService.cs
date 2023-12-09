using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace AadhaarSplitterAPI.Services
{
    public interface IAadhaarService
    {
        List<string> ExtractAadhaarNumbers(string rawText);
    }

    public class AadhaarService : IAadhaarService
    {
        private readonly ILogger<AadhaarService> _logger;

        public AadhaarService(ILogger<AadhaarService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Extracts Aadhaar numbers from the given raw text.
        /// </summary>
        /// <param name="rawText">The raw text to extract Aadhaar numbers from.</param>
        /// <returns>A list of valid Aadhaar numbers.</returns>      
       
        public List<string> ExtractAadhaarNumbers(string rawText)
        {
            try
            {
                string[] lines = rawText.Split('\n');

                List<string> validAadhaarNumbers = new List<string>();

                foreach (string line in lines)
                {
                    
                    string[] blocks = line.Split(' ');

                    foreach (string block in blocks)
                    {                        
                        MatchCollection matches = Regex.Matches(block, @"\b\d{12}\b");

                        foreach (Match match in matches)
                        {
                            string aadhaarNumber = ValidateAndManipulate(match.Value);

                            if (IsValidAadhaarNumber(aadhaarNumber))
                            {
                                validAadhaarNumbers.Add(aadhaarNumber);
                            }
                        }
                    }
                }

                _logger.LogInformation("Extracted Aadhaar numbers: {AadhaarNumbers}", string.Join(", ", validAadhaarNumbers));

                return validAadhaarNumbers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting Aadhaar numbers");
                throw; // Re-throw the exception for higher-level handling
            }
        }

        /*        public List<string> ExtractAadhaarNumbers(string rawText)
        {
            try
            {
                string pattern = @"\b\d{12}\b";
                Regex regex = new Regex(pattern);

                MatchCollection matches = regex.Matches(rawText);

                List<string> validAadhaarNumbers = new List<string>();
                foreach (Match match in matches)
                {
                    string aadhaarNumber = ValidateAndManipulate(match.Value);

                    if (IsValidAadhaarNumber(aadhaarNumber))
                    {
                        validAadhaarNumbers.Add(aadhaarNumber);
                    }
                }

                _logger.LogInformation("Extracted Aadhaar numbers: {AadhaarNumbers}", string.Join(", ", validAadhaarNumbers));

                return validAadhaarNumbers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting Aadhaar numbers");
                throw; // Re-throw the exception for higher-level handling
            }
        }
*/

        private string ValidateAndManipulate(string aadhaarNumber)
        {
            try
            {
                aadhaarNumber = aadhaarNumber
                    .Replace('O', '0')
                    .Replace('B', '3')
                    .Replace('Z', '2')
                    .Replace('I', '1');

                return aadhaarNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating and manipulating Aadhaar number");
                throw; // Re-throw the exception for higher-level handling
            }
        }

        private bool IsValidAadhaarNumber(string aadhaarNumber)
        {
            try
            {
                if (!Is12DigitNumber(aadhaarNumber))
                {
                    _logger.LogWarning("Invalid Aadhaar number length: {AadhaarNumber}", aadhaarNumber);
                    return false;
                }

                if (!VerifyWithVerhoeff(aadhaarNumber))
                {
                    _logger.LogWarning("Invalid Aadhaar number: {AadhaarNumber}", aadhaarNumber);
                    return false;
                }

                // Additional validations can be added here

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating Aadhaar number");
                throw; // Re-throw the exception for higher-level handling
            }
        }

        private bool Is12DigitNumber(string aadhaarNumber)
        {
            try
            {
                string pattern = @"^\d{12}$";
                return Regex.IsMatch(aadhaarNumber, pattern);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating Aadhaar number length");
                throw; // Re-throw the exception for higher-level handling
            }
        }

        private bool VerifyWithVerhoeff(string aadhaarNumber)
        {
            try
            {
                int[][] d =
                {
            new [] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
            new [] {1, 2, 3, 4, 0, 6, 7, 8, 9, 5},
            new [] {2, 3, 4, 0, 1, 7, 8, 9, 5, 6},
            new [] {3, 4, 0, 1, 2, 8, 9, 5, 6, 7},
            new [] {4, 0, 1, 2, 3, 9, 5, 6, 7, 8},
            new [] {5, 9, 8, 7, 6, 0, 4, 3, 2, 1},
            new [] {6, 5, 9, 8, 7, 1, 0, 4, 3, 2},
            new [] {7, 6, 5, 9, 8, 2, 1, 0, 4, 3},
            new [] {8, 7, 6, 5, 9, 3, 2, 1, 0, 4},
            new [] {9, 8, 7, 6, 5, 4, 3, 2, 1, 0}
        };

                int[][] p =
                {
            new [] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
            new [] {1, 5, 7, 6, 2, 8, 3, 0, 9, 4},
            new [] {5, 8, 0, 3, 7, 9, 6, 1, 4, 2},
            new [] {8, 9, 1, 6, 0, 4, 3, 5, 2, 7},
            new [] {9, 4, 5, 3, 1, 2, 6, 8, 7, 0},
            new [] {4, 2, 8, 6, 5, 7, 3, 9, 0, 1},
            new [] {2, 7, 9, 3, 8, 0, 6, 4, 1, 5},
            new [] {7, 0, 4, 6, 9, 1, 3, 2, 5, 8}
        };

                int[] inv = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };

                int c = aadhaarNumber
                    .Select(ch => int.Parse(ch.ToString()))
                    .Aggregate(0, (current, t) => d[current][p[current % 8][t]]);

                return c == 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying Aadhaar number with Verhoeff algorithm");
                throw; 
            }
        }
    }
}




