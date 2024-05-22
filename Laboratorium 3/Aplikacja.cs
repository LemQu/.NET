using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Aplikacja1
{
    public partial class Form1 : Form
    {
        private SymmetricAlgorithm algorithm;
        private byte[] key;
        private byte[] iv;

        public Form1()
        {
            InitializeComponent();
            algorytmComboBox.Items.AddRange(new string[] { "AES", "DES", "TripleDES" });
            algorytmComboBox.SelectedIndex = 0;
        }

        private void buttonGenerateKeys_Click(object sender, EventArgs e)
        {
            switch (algorytmComboBox.SelectedItem.ToString())
            {
                case "AES":
                    algorithm = Aes.Create();
                    break;
                case "DES":
                    algorithm = DES.Create();
                    break;
                case "TripleDES":
                    algorithm = TripleDES.Create();
                    break;
            }

            algorithm.GenerateKey();
            algorithm.GenerateIV();
            key = algorithm.Key;
            iv = algorithm.IV;

            textBoxKey.Text = BitConverter.ToString(key).Replace("-", "");
            textBoxIV.Text = BitConverter.ToString(iv).Replace("-", "");
        }

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            var plainText = textBoxPlainText.Text;
            var plainBytes = Encoding.ASCII.GetBytes(plainText);

            var stopwatch = Stopwatch.StartNew();
            var cipherBytes = Encrypt(plainBytes);
            stopwatch.Stop();
            labelEncryptionTime.Text = $"Encryption Time: {stopwatch.ElapsedMilliseconds} ms";

            textBoxCipherText.Text = Encoding.ASCII.GetString(cipherBytes);
            textBoxPlainTextHex.Text = BitConverter.ToString(plainBytes).Replace("-", "");
            textBoxCipherTextHex.Text = BitConverter.ToString(cipherBytes).Replace("-", "");
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            var cipherBytes = Encoding.ASCII.GetBytes(textBoxCipherText.Text);

            var stopwatch = Stopwatch.StartNew();
            var plainBytes = Decrypt(cipherBytes);
            stopwatch.Stop();
            labelDecryptionTime.Text = $"Decryption Time: {stopwatch.ElapsedMilliseconds} ms";

            textBoxPlainText.Text = Encoding.ASCII.GetString(plainBytes);
            textBoxPlainTextHex.Text = BitConverter.ToString(plainBytes).Replace("-", "");
        }

        private byte[] Encrypt(byte[] plainBytes)
        {
            using (var encryptor = algorithm.CreateEncryptor(key, iv))
            {
                return PerformCryptography(plainBytes, encryptor);
            }
        }

        private byte[] Decrypt(byte[] cipherBytes)
        {
            using (var decryptor = algorithm.CreateDecryptor(key, iv))
            {
                return PerformCryptography(cipherBytes, decryptor);
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform transform)
        {
            using (var cryptoStream = new System.IO.MemoryStream())
            {
                using (var cryptoTransform = new CryptoStream(cryptoStream, transform, CryptoStreamMode.Write))
                {
                    cryptoTransform.Write(data, 0, data.Length);
                    cryptoTransform.FlushFinalBlock();
                    return cryptoStream.ToArray();
                }
            }
        }
    }
}