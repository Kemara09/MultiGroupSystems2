using System;
using System.Security.Cryptography;

namespace MultiGroupSystemsTester
{
 
    public static class PasswordHasher
    {
        private const int SaltSize = 16;   
        private const int HashSize = 32;   
        private const int Iterations = 100000;

        public static string HashPassword(string password)
        {
            using (var rfc = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] salt = rfc.Salt;
                byte[] hash = rfc.GetBytes(HashSize);
                return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
            }
        }

      
        public static bool VerifyPassword(string password, string storedHash)
        {

            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            try
            {
                string[] parts = storedHash.Split('.');

                if (parts.Length != 3)
                    return false;

                int iterations;

                if (!int.TryParse(parts[0], out iterations) ||
                    iterations < 10000 ||
                    iterations > 10000000)
                {
                    return false;
                }

                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] hash = Convert.FromBase64String(parts[2]);

                if (salt.Length != SaltSize || hash.Length != HashSize)
                    return false;

                using (var rfc = new Rfc2898DeriveBytes(
                    password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    byte[] testHash = rfc.GetBytes(hash.Length);
                    return SlowEquals(hash, testHash);
                }
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        

        
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
}