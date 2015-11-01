namespace ProjectForTestingPurposes
{
    using System;
    using System.Data.Entity;
    using System.Net.Http;

    using Eugenie.Data;
    using Eugenie.Data.Migrations;
    using Eugenie.Data.Models;
    using Eugenie.Server.Api;

    using Microsoft.Owin.Hosting;

    public class Program
    {
        private static void Main(string[] args)
        {
            string baseAddress = "http://localhost:10000/";

            WebApp.Start<Startup>(url: baseAddress);
            {
                // Create HttpCient and make a request to api/values
                HttpClient client = new HttpClient();

                HttpResponseMessage response = client.GetAsync(baseAddress + "api/values").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            Console.ReadLine();
        }
    }
}