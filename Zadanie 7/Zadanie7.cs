using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string sourceFilePath = "sourceFile.txt";
        string destinationFilePath = "destinationFile.txt";

        // Generowanie pliku
        GenerateLargeFile(sourceFilePath, 300);

        // Testowanie czasu kopiowania
        TestFileStreamCopy(sourceFilePath, destinationFilePath);

        File.Delete(sourceFilePath);
        File.Delete(destinationFilePath);

        Console.WriteLine("Testy zakończone.");
    }

    static void GenerateLargeFile(string filePath, int sizeInMB)
    {
        Console.WriteLine("Generowanie pliku o wielkości {0}MB...", sizeInMB);

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB bufor
            Random random = new Random();
            long fileSize = sizeInMB * 1024L * 1024L;

            while (fileStream.Length < fileSize)
            {
                random.NextBytes(buffer);
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }

        Console.WriteLine("Plik został wygenerowany: {0}", filePath);
    }

    static void TestFileStreamCopy(string sourceFilePath, string destinationFilePath)
    {
        Console.WriteLine("Rozpoczęcie testu kopiowania z użyciem FileStream...");

        Stopwatch stopwatch = Stopwatch.StartNew();

        using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
        using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB bufor
            int bytesRead;

            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
            }
        }

        stopwatch.Stop();

        Console.WriteLine("Czas kopiowania z użyciem FileStream: {0} ms", stopwatch.ElapsedMilliseconds);
    }
}