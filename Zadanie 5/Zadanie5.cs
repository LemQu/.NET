using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Wybierz operację:");
            Console.WriteLine("1. Zapisz dane do pliku binarnego");
            Console.WriteLine("2. Odczytaj dane z pliku binarnego");
            Console.WriteLine("3. Wyjście");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ZapiszDoPlikuBinarnego();
                    break;
                case "2":
                    OdczytajZPlikuBinarnego();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    static void ZapiszDoPlikuBinarnego()
    {
        try
        {
            // Zapisywanie podawanych informacji
            Console.Write("Podaj imię: ");
            string imie = Console.ReadLine();
            Console.Write("Podaj wiek: ");
            int wiek = int.Parse(Console.ReadLine());
            Console.Write("Podaj adres: ");
            string adres = Console.ReadLine();

            // Zapis danych do pliku
            using (FileStream fileStream = new FileStream("dane.bin", FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                writer.Write(imie);
                writer.Write(wiek);
                writer.Write(adres);
            }

            Console.WriteLine("Dane zostały zapisane do pliku binarnego.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Wystąpił błąd: " + ex.Message);
        }
    }

    static void OdczytajZPlikuBinarnego()
    {
        try
        {
            using (FileStream fileStream = new FileStream("dane.bin", FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                string imie = reader.ReadString();
                int wiek = reader.ReadInt32();
                string adres = reader.ReadString();

                // Wyświetlanie danych
                Console.WriteLine("Imię: " + imie);
                Console.WriteLine("Wiek: " + wiek);
                Console.WriteLine("Adres: " + adres);
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Plik nie istnieje.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Wystąpił błąd: " + ex.Message);
        }
    }
}
