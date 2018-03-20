namespace Unic.Flex.Core.Utilities
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Security related utilities.
    /// </summary>
    public static class SecurityUtil
    {
        /// <summary>
        /// The salt
        /// </summary>
        private const string Salt = "Un!CFl3x";

        /// <summary>
        /// Gets the MD5 hash.
        /// </summary>
        /// <param name="md5Hash">The MD5 hash.</param>
        /// <param name="input">The input.</param>
        /// <returns>Hashed input</returns>
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Add some salt to the input
            input = string.Join("_", input, Salt);

            // Convert the input string to a byte array and compute the hash.
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes and create a string.
            var stringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Verifies the MD5 hash.
        /// </summary>
        /// <param name="md5Hash">The MD5 hash.</param>
        /// <param name="input">The input.</param>
        /// <param name="hash">The hash.</param>
        /// <returns>True if the has is correct, false otherwise</returns>
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        public static string GenerateHash(string content)
        {
            using (var crypt = new SHA1Managed())
            {
                var contentWithSaltBytes = Encoding.UTF8.GetBytes(content + Salt);
                var crypto = crypt.ComputeHash(contentWithSaltBytes, 0, contentWithSaltBytes.Length);
                return Convert.ToBase64String(crypto);
            }
        }
    }
}
