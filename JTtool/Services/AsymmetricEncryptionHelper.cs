using System;
using System.Security.Cryptography;
using System.Text;

public static class AsymmetricEncryptionHelper
{
    // 產生新的公開金鑰和私密金鑰對
    public static void GenerateKeyPair()
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            string publicKey = rsa.ToXmlString(false);  // 公開金鑰
            string privateKey = rsa.ToXmlString(true);  // 私密金鑰
        }
    }

    // 使用公開金鑰加密資料
    public static string Encrypt(string data, string publicKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] encryptedBytes = rsa.Encrypt(dataBytes, true);
            return Convert.ToBase64String(encryptedBytes);
        }
    }

    // 使用私密金鑰解密資料
    public static string Decrypt(string encryptedData, string privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKey);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, true);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
