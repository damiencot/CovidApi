using CovidApi.model;
using CovidApi.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidApi
{
    class Program
    {
        private const string GLOBAL_DATA_URL = "https://coronavirusapi-france.now.sh/FranceLiveGlobalData";
        private const string GLOBAL_DATA_TOKEN = "FranceGlobalLiveData";

        private const string GLOBAL_BY_DEPARTEMENT_URL = "https://coronavirusapi-france.now.sh/LiveDataByDepartement";
        private const string ALL_DEPARTEMENTS_URL = "https://coronavirusapi-france.now.sh/AllLiveData";
        private const string ALL_DEPARTEMENTS_TOKEN = "allLiveFranceData";

        private static HttpClient client;
        private static Printer printer;

        static async Task<string> getDataAsync(string requestUri)
        {
            string data = string.Empty;
            var response = await client.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsStringAsync();
            }
            return data;
        }
        static void Main(string[] args)
        {

            client = new HttpClient();

            printer = new Printer
            {
                TableWidth = 100
            };

            Console.WriteLine("Covid-19 France!");
            while (true)
            {
                args = Console.ReadLine().Split(' ');
                var command = args[0];
                args = args.Skip(1).ToArray();

                switch (command)
                {
                    case "france":
                        runAsync(GLOBAL_DATA_URL, GLOBAL_DATA_TOKEN).GetAwaiter().GetResult();
                        break;
                    case "departements":
                        runAsync(ALL_DEPARTEMENTS_URL, ALL_DEPARTEMENTS_TOKEN).GetAwaiter().GetResult();
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        static async Task runAsync(string requestUri, string token)
        {
            var json = await getDataAsync(GLOBAL_DATA_URL);
            var data = JObject.Parse(json).SelectToken(token).ToObject<List<Data>>();


            switch (requestUri)
            {
                case GLOBAL_DATA_URL:
                    printGlobalData(data.First());
                    break;
                case GLOBAL_BY_DEPARTEMENT_URL:
                    break;
                case ALL_DEPARTEMENTS_URL:
                    break;
                default:
                    Console.WriteLine("Erreur durant l'exécution");
                    break;
            }

        }

        static void printGlobalData(Data data)
        {
            Console.WriteLine("Voici les données globales connus pour la france");
            printData(data);
        }

        static void printData(Data data)
        {
            printer.Line();
            printer.Row("Date", "Pays", "Décés", "Guéris", "Hospitalisés", "Réanimation");
            printer.Line();
            printer.Row(data.date.ToString("dd/MM/yyyy"), data.nom, data.deces, data.gueris, data.hospitalises, data.reanimation);
            printer.Line();
        }
    }
}
