using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string originalFile = "test.txt";
        string encryptedFile = "test.enc";
        string decryptedFile = "test.dec";

        GenerateKeys(out RSAParameters publicKey, out RSAParameters privateKey);

        EncryptFile(originalFile, encryptedFile, publicKey);
        DecryptFile(encryptedFile, decryptedFile, privateKey);

        Console.WriteLine("Plik został zaszyfrowany i odszyfrowany.");
    }

    static void GenerateKeys(out RSAParameters publicKey, out RSAParameters privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            publicKey = rsa.ExportParameters(false);
            privateKey = rsa.ExportParameters(true);
        }
    }

    static void EncryptFile(string inputFile, string outputFile, RSAParameters publicKey)
    {
        byte[] fileBytes = File.ReadAllBytes(inputFile);

        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(publicKey);

            byte[] encryptedBytes = rsa.Encrypt(fileBytes, false);

            File.WriteAllBytes(outputFile, encryptedBytes);
        }
    }

    static void DecryptFile(string inputFile, string outputFile, RSAParameters privateKey)
    {
        byte[] encryptedBytes = File.ReadAllBytes(inputFile);

        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(privateKey);

            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);

            File.WriteAllBytes(outputFile, decryptedBytes);
        }
    }
}
