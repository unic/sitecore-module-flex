namespace Unic.Flex.Core.Utilities
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class SecurityUtil
    {
        private const string Salt = "Un!CFl3x";

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            input = string.Join("_", input, Salt);
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var stringBuilder = new StringBuilder();
            foreach (var t in data)
            {
                stringBuilder.Append(t.ToString("x2"));
            }
            
            return stringBuilder.ToString();
        }

        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            var hashOfInput = GetMd5Hash(md5Hash, input);
            var comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }

        public static string GenerateHash(string content, string salt)
        {
            using (var crypt = new SHA1Managed())
            {
                var contentWithSaltBytes = Encoding.UTF8.GetBytes(content + salt);
                var crypto = crypt.ComputeHash(contentWithSaltBytes, 0, contentWithSaltBytes.Length);
                return Convert.ToBase64String(crypto);
            }
        }
    }
}
