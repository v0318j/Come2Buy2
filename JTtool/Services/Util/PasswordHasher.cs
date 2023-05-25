using JTtool.Models;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace JTtool.Services.Util
{
    public class PasswordHasher
    {
        public string HashPassword(string password)
        {
            byte[] salt = GenerateSalt();

            byte[] saltedPassword = CombineSaltAndPassword(salt, password);

            byte[] hash = ComputeHash(saltedPassword);

            byte[] hashedPassword = CombineSaltAndHash(salt, hash);

            string hashedPasswordBase64 = Convert.ToBase64String(hashedPassword);

            return hashedPasswordBase64;
        }

        public bool VerifyPassword(string password, string hashedPasswordBase64)
        {
            byte[] hashedPassword = Convert.FromBase64String(hashedPasswordBase64);

            byte[] salt = ExtractSalt(hashedPassword);
            byte[] hash = ExtractHash(hashedPassword);

            byte[] saltedPassword = CombineSaltAndPassword(salt, password);

            byte[] computedHash = ComputeHash(saltedPassword);

            bool passwordMatch = CompareHashes(hash, computedHash);
            if (!passwordMatch)
            {
                throw new CustomException("密碼驗證失敗");
            }

            return passwordMatch;
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(salt);
            }
            return salt;
        }

        private byte[] CombineSaltAndPassword(byte[] salt, string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];

            Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

            return saltedPassword;
        }

        private byte[] ComputeHash(byte[] data)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                return hashAlgorithm.ComputeHash(data);
            }
        }

        private byte[] CombineSaltAndHash(byte[] salt, byte[] hash)
        {
            byte[] hashedPassword = new byte[salt.Length + hash.Length];

            Buffer.BlockCopy(salt, 0, hashedPassword, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, hashedPassword, salt.Length, hash.Length);

            return hashedPassword;
        }

        private byte[] ExtractSalt(byte[] hashedPassword)
        {
            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashedPassword, 0, salt, 0, salt.Length);
            return salt;
        }

        private byte[] ExtractHash(byte[] hashedPassword)
        {
            byte[] hash = new byte[hashedPassword.Length - 16];
            Buffer.BlockCopy(hashedPassword, 16, hash, 0, hash.Length);
            return hash;
        }

        private bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }

            return true;
        }
    }
}