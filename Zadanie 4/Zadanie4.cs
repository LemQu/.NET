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
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                
                while ((line = reader.ReadLine()) != null)
                {
                    for (int i = line.Length - 1; i >= 0; i--)
                    {
                        Console.Write(line[i]);
                    }
                    Console.WriteLine();
                }
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