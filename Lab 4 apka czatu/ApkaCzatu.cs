using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

class ChatClient
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF"); // 32 bity AES-256
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("ABCDEF0123456789"); // 16 bitów AES

    static void Main(string[] args)
    {
        TcpClient client = new TcpClient("127.0.0.1", 8888);
        NetworkStream stream = client.GetStream();

        Thread readThread = new Thread(() => ReadMessages(stream));
        readThread.Start();

        Console.WriteLine("Connected to server. You can start typing messages...");
        while (true)
        {
            string message = Console.ReadLine(); // odczyt wiadomości z konsoli
            byte[] buffer = EncryptMessage(message); // szyfrowanie
            stream.Write(buffer, 0, buffer.Length); // wysyłka wiadomości
        }
    }

    public static void ReadMessages(NetworkStream stream)
    {
        while (true)
        {
            try
            {
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead == 0) break;

                string message = DecryptMessage(buffer, bytesRead);
                Console.WriteLine("Received: " + message);
            }
            catch (Exception)
            {
                break;
            }
        }
    }

    public static byte[] EncryptMessage(string message)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new System.IO.MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new System.IO.StreamWriter(cs))
                    {
                        sw.Write(message);
                    }
                    return ms.ToArray();
                }
            }
        }
    }

    public static string DecryptMessage(byte[] buffer, int bytesRead)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new System.IO.MemoryStream(buffer, 0, bytesRead))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (var sr = new System.IO.StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
//Przy uruchomieniu wielu okienek czatu można komunikować się pomiędzy sobą, serwer będzie zbierał wszystkie wiadomości
