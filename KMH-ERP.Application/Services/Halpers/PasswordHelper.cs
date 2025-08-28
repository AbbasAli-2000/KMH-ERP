using System.Security.Cryptography;
using System.Text;

namespace KMH_ERP.Infrastructure.Utilities.Halpers
{
    public static class PasswordHelper
    {

        // Hash password using SHA256 (simple hashing)
        public static string HashPassword(string password)
        {

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        //Verify password
        public static bool VerifyPassword(string password, string hashPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hashPassword);
        }

        //  Generate Salt (extra security layer)
        public static string GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        //  Hash with Salt
        public static string HashPasswordWithSalt(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combinedBytes = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hash);
        }

        //  Verify with Salt
        public static bool VerifyPasswordWithSalt(string password, string salt, string hashedPassword)
        {
            var hashOfInput = HashPasswordWithSalt(password, salt);
            return hashOfInput == hashedPassword;
        }
    }
}
