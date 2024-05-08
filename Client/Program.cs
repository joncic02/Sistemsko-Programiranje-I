using System;
using System.Net.Http;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> keywords = new List<string>
            {
                "dozvola",
                "baza",
                "pokusaj",
                "proba",
                "baza"
            };

            Thread[] threads = new Thread[keywords.Count];

            int i = 0;

            foreach(string keyword in keywords)
            {
                threads[i] = new Thread(() => SendRequest(keyword));
                threads[i].Start();
                i++;
            }
            foreach(Thread thread in threads)
            {
                thread.Join();
            }
        }

        private static void SendRequest(string keyword)
        {
            string url = $"http://localhost:5050/{keyword}";
            using(HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();

                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Odgovor servera na zahtev sa kljucnom reci '{keyword}':");
                    Console.WriteLine(responseBody);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Greska prilikom slanja zahteva za fajlovima sa kljucnom reci '{keyword}': { ex.Message}");
                }
            }
        }
    }
}