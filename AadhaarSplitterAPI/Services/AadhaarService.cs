using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AadhaarSplitterAPI.Services
{
    public interface IAadhaarService
    {
        List<string> ExtractAadhaarNumbers(string rawText);
    }

    public class AadhaarService : IAadhaarService
    {
        public List<string> ExtractAadhaarNumbers(string rawText)
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

           // Console.WriteLine("Extracted Aadhaar numbers: " + string.Join(", ", validAadhaarNumbers));
            return validAadhaarNumbers;
        }

        private string ValidateAndManipulate(string aadhaarNumber)
        {
            aadhaarNumber = aadhaarNumber
                .Replace('O', '0')
                .Replace('B', '3')
                .Replace('Z', '2')
                .Replace('I', '1');

            return aadhaarNumber;
        }

        private bool IsValidAadhaarNumber(string aadhaarNumber)
        {
            if (!Is12DigitNumber(aadhaarNumber))
            {
                return false;
            }

            if (!VerifyWithVerhoeff(aadhaarNumber))
            {
                return false;
            }

            // Additional validations can be added here

            return true;
        }

        private bool Is12DigitNumber(string aadhaarNumber)
        {
            string pattern = @"^\d{12}$";
            return Regex.IsMatch(aadhaarNumber, pattern);
        }

        private bool VerifyWithVerhoeff(string aadhaarNumber)
        {
            int[][] d = {
                new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                new int[] {1, 2, 3, 4, 0, 6, 7, 8, 9, 5},
                new int[] {2, 3, 4, 0, 1, 7, 8, 9, 5, 6},
                new int[] {3, 4, 0, 1, 2, 8, 9, 5, 6, 7},
                new int[] {4, 0, 1, 2, 3, 9, 5, 6, 7, 8},
                new int[] {5, 9, 8, 7, 6, 0, 4, 3, 2, 1},
                new int[] {6, 5, 9, 8, 7, 1, 0, 4, 3, 2},
                new int[] {7, 6, 5, 9, 8, 2, 1, 0, 4, 3},
                new int[] {8, 7, 6, 5, 9, 3, 2, 1, 0, 4},
                new int[] {9, 8, 7, 6, 5, 4, 3, 2, 1, 0}
            };

            int[][] p = {
                new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                new int[] {1, 5, 7, 6, 2, 8, 3, 0, 9, 4},
                new int[] {5, 8, 0, 3, 7, 9, 6, 1, 4, 2},
                new int[] {8, 9, 1, 6, 0, 4, 3, 5, 2, 7},
                new int[] {9, 4, 5, 3, 1, 2, 6, 8, 7, 0},
                new int[] {4, 2, 8, 6, 5, 7, 3, 9, 0, 1},
                new int[] {2, 7, 9, 3, 8, 0, 6, 4, 1, 5},
                new int[] {7, 0, 4, 6, 9, 1, 3, 2, 5, 8}
            };

            int[] inv = new int[] { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };

            int c = 0;
            int[] myArray = new int[aadhaarNumber.Length];
            for (int i = 0; i < aadhaarNumber.Length; i++)
            {
                myArray[i] = int.Parse(aadhaarNumber[i].ToString());
            }

            for (int i = 0; i < myArray.Length; i++)
            {
                c = d[c][p[i % 8][myArray[i]]];
            }

            return c == 0;
        }
    }
}
