using JianShuCore.CryptoCore;
using JianShuCore.Interface;
using System;
using System.Text;

namespace JianShuCore.Provider
{
    public sealed class MD5CryptoProvider : IProvider
    {
        /// <summary>
        /// Compute Hash
        /// </summary>
        /// <param name="source">Byte[]</param>
        /// <returns>Byte[]</returns>
        public Byte[] ComputeHash(Byte[] source)
        {
            return MD5Core.GetHash(source);
        }

        /// <summary>
        /// Compute Hash
        /// </summary>
        /// <param name="content">String</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Byte[]</returns>
        public Byte[] ComputeHash(String content, Encoding encoding)
        {
            return MD5Core.GetHash(encoding.GetBytes(content));
        }

        /// <summary>
        /// Converter Hash to string
        /// </summary>
        /// <param name="hash">Byte[]</param>
        /// <returns>String</returns>
        public String HashToString(Byte[] hash)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        /// <summary>
        /// Compute hash and Convert to String
        /// </summary>
        /// <param name="source">Byte[]</param>
        /// <returns>String</returns>
        public String ComputeHashAsString(Byte[] source)
        {
            return HashToString(ComputeHash(source));
        }

        /// <summary>
        /// Compute hash and Convert to String
        /// </summary>
        /// <param name="content">String</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>String</returns>
        public String ComputerHashAsString(String content, Encoding encoding = null)
        {
            Encoding enc = encoding;
            if (enc == null)
            {
                enc = Encoding.UTF8;
            }

            return HashToString(ComputeHash(content, enc));
        }

        /// <summary>
        /// Initialize the Class
        /// </summary>
        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
