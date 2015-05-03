using System;
using Nancy.Hosting.Self;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Nancy.SelfHost
{
    class MainClass
    {
        public static string serverUrl = "http://localhost:9090";

        public static void Main(string[] args)
        {
            Console.WriteLine("Loading...");

            using (var host = new NancyHost(
                                  new HostConfiguration
                {
                    UrlReservations = new UrlReservations { CreateAutomatically = true }
                },
                                  new Uri(serverUrl)))
            { 
                host.Start();
                Console.WriteLine("Started."); 


                HttpClient client = new HttpClient();

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(serverUrl + "/api/sys/version"),
                    Method = HttpMethod.Get,
                };
                
                var response = client.SendAsync(request).Result; 

                Console.WriteLine("Sys call:" + (response.IsSuccessStatusCode ? "Ok" : "Fail"));
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Running... press Enter to quit");
                    Console.ReadLine();
                }
            }
        }
    }
}
