using System.Security.Cryptography;
using System.Text;

namespace Launcher.Core
{
    public class Crypto
    {
        public static string MD5(string source)
        {
            using (MD5 hash = System.Security.Cryptography.MD5.Create())
            {
                return Compute(hash, source);
            }
        }

        public static string SHA256(string source)
        {
            using (SHA256 hash = System.Security.Cryptography.SHA256.Create())
            {
                return Compute(hash, source);
            }
        }

        private static string Compute(HashAlgorithm hash, string source)
        {
            byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }
            return builder.ToString().ToLower();
        }
    }
}
