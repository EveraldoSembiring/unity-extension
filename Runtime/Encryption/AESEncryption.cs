using System;
using System.Security.Cryptography;
using System.Text;

namespace UnityExtension
{
    public class AESEncryption : IEncryption
    {
        byte[] keyBytes, ivBytes;

        public AESEncryption(byte[] keyBytes, byte[] ivBytes)
        {
            this.keyBytes = keyBytes;
            this.ivBytes = ivBytes;
        }

        public static byte[] GenerateIVBytes()
        {
            byte[] ivBytes = new byte[16];
            System.Random random = new System.Random();
            random.NextBytes(ivBytes);
            return ivBytes;
        }

        public static byte[] GenerateKeyBytes(string key)
        {
            int sum = 0;
            foreach(char curChar in key)
            {
                sum += curChar;
            }

            byte[] keyBytes = new byte[16];
            System.Random random = new System.Random(sum);
            random.NextBytes(keyBytes);
            return keyBytes;
        }

        public string Decrypt(string cipherText)
        {
            int endOfIVBytes = ivBytes.Length / 2;

            string ivString = cipherText.Substring(0, endOfIVBytes);
            byte[] extractedIVBytes = Encoding.Unicode.GetBytes(ivString);

            string encryptedString = cipherText.Substring(endOfIVBytes);

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(keyBytes, extractedIVBytes);

            byte[] inputBuffer = Convert.FromBase64String(encryptedString);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string plainText = Encoding.Unicode.GetString(outputBuffer);

            return plainText;
        }

        public string Encrypt(string plainText)
        {
            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(keyBytes, ivBytes);
            byte[] inputBuffer = Encoding.Unicode.GetBytes(plainText);
            byte[] outputBuffer = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            string ivString = Encoding.Unicode.GetString(ivBytes);
            string encryptedString = Convert.ToBase64String(outputBuffer);

            string retval = ivString + encryptedString;
            return retval;
        }
    }
}