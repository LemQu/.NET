using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Ścieżka do pliku
        string filePath = "Zadanie3/tekst.txt";

        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fileStream.Length];

                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                string fileContent = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine("Zawartość pliku:");
                Console.WriteLine(fileContent);
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Plik nie został odnaleziony.");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Wystąpił błąd wejścia/wyjścia: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Wystąpił nieoczekiwany błąd: " + ex.Message);
        }
    }
}
