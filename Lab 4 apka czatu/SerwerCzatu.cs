using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Security.Cryptography;

class ChatServer
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF"); // 32 bity AES-256, z generatora
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("ABCDEF0123456789"); // 16 bit AES, z generatora

    static List<TcpClient> clients = new List<TcpClient>();
    static object _lock = new object();

    public static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            lock (_lock) clients.Add(client); // dodanie nowego klienta do listy
            Console.WriteLine("Client connected...");

            //nowywątek dla obsługi klienta
            Thread t = new Thread(HandleClient);
            t.Start(client);
        }
    }

    public static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();

        while (true)
        {
            try
            {
                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead == 0) break;

                // odszyfrowanie wiadomości
                string message = DecryptMessage(buffer, bytesRead);
                Console.WriteLine("Received: " + message);

                BroadcastMessage(message, client);
            }
            catch (Exception)
            {
                break;
            }
        }

        lock (_lock) clients.Remove(client);
        client.Close();
        Console.WriteLine("Client disconnected...");
    }

    public static void BroadcastMessage(string message, TcpClient excludeClient)
    {
        byte[] buffer = EncryptMessage(message); // szyfrowanie wiadomosci

        lock (_lock)
        {
            foreach (TcpClient client in clients)
            {
                if (client != excludeClient)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                }
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
