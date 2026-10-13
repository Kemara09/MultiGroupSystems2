using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiGroupSystemsTester
{
   
    public static class PasswordPolicy
    {
        public const int MinLength = 8;
        public const int MaxLength = 64;

        
        private static readonly HashSet<string> Blocked = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "password", "password1", "password1!", "password123", "password123!", "passw0rd", "p@ssw0rd",
            "p@ssword1", "p@ssword123", "qwerty123", "qwerty123!", "welcome1", "welcome123", "welcome1!",
            "letmein123", "admin123", "admin123!", "iloveyou1", "12345678", "123456789", "1234567890",
            "abc12345", "abcd1234", "multigroup1", "multigroup123", "multigroup1!", "multigroup@1"
        };

        
        public static bool Validate(string password, out string error)
        {
            error = null;

            if (string.IsNullOrEmpty(password))
            {
                error = "Please enter a password.";
                return false;
            }

            var missing = new List<string>();
            if (password.Length < MinLength) missing.Add("at least " + MinLength + " characters");
            if (!password.Any(c => c >= 'A' && c <= 'Z')) missing.Add("an uppercase letter");
            if (!password.Any(c => c >= 'a' && c <= 'z')) missing.Add("a lowercase letter");
            if (!password.Any(c => c >= '0' && c <= '9')) missing.Add("a number");
            if (!password.Any(IsSpecial)) missing.Add("a special character (for example ! @ # $ %)");

            if (missing.Count > 0)
            {
                error = "Your password needs " + JoinNaturally(missing) + ".";
                return false;
            }

            if (password.Length > MaxLength)
            {
                error = "Your password can be at most " + MaxLength + " characters long.";
                return false;
            }

            if (password.Any(char.IsWhiteSpace))
            {
                error = "Your password can't contain spaces.";
                return false;
            }

            if (Blocked.Contains(password))
            {
                error = "That password is too common. Please choose something harder to guess.";
                return false;
            }

            return true;
        }

      
        private static bool IsSpecial(char c)
        {
            bool letterOrDigit = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');
            return !letterOrDigit && !char.IsWhiteSpace(c);
        }

       
        private static string JoinNaturally(List<string> items)
        {
            if (items.Count == 1) return items[0];
            return string.Join(", ", items.Take(items.Count - 1)) + " and " + items[items.Count - 1];
        }
    }
}