using System.IO;
using System.Security.Cryptography;

namespace SecretStore.Core;

public static class Encryptor {
    private const int DERIVATION_ITERATIONS = 1000;

    public static byte[] Encrypt(byte[] data, string passPhrase) {
        var saltStringBytes = RandomNumberGenerator.GetBytes(16);
        var ivStringBytes = RandomNumberGenerator.GetBytes(16);
        using var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DERIVATION_ITERATIONS, HashAlgorithmName.SHA256);
        var keyBytes = password.GetBytes(16);
        using var aes = MakeAes(keyBytes, ivStringBytes);
        using var encryptor = aes.CreateEncryptor(keyBytes, ivStringBytes);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(data, 0, data.Length);
        cryptoStream.FlushFinalBlock();
        var output = new MemoryStream();
        output.Write(saltStringBytes);
        output.Write(ivStringBytes);
        output.Write(memoryStream.ToArray());
        memoryStream.Close();
        cryptoStream.Close();
        return output.ToArray();
    }

    public static byte[] Decrypt(byte[] data, string passPhrase) {
        var saltStringBytes = data[..16];
        var ivStringBytes = data[16..32];
        var cipherTextBytes = data[32..];
        using var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DERIVATION_ITERATIONS, HashAlgorithmName.SHA256);
        var keyBytes = password.GetBytes(16);
        using var aes = MakeAes(keyBytes, ivStringBytes);
        using var decryptor = aes.CreateDecryptor(keyBytes, ivStringBytes);
        using var memoryStream = new MemoryStream(cipherTextBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var outputStream = new MemoryStream();
        cryptoStream.CopyTo(outputStream);
        return outputStream.ToArray();
    }

    private static Aes MakeAes(byte[] key, byte[] iv) {
        var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.BlockSize = 128;
        aes.FeedbackSize = 128;
        aes.KeySize = 128;
        return aes;
    }
}
