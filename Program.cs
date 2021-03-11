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

        private const string DATA_BY_DEPARTEMENT_URL = "https://coronavirusapi-france.now.sh/LiveDataByDepartement";
        private const string DATA_BY_DEPARTEMENT_TOKEN = "LiveDataByDepartement";

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
                TableWidth = 96
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
                    case "departement":
                        if(args.Length == 0)
                        {
                            Console.WriteLine("Le nom du département n'a pas été spécifié");
                            return;
                        }
                        var parameters = new Dictionary<string, string>
                        {
                            {"Departement", args[0].toTitleCase() }
                        };
                        runAsync(DATA_BY_DEPARTEMENT_URL, DATA_BY_DEPARTEMENT_TOKEN, parameters).GetAwaiter().GetResult();
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        static async Task runAsync(string requestUri, string token, Dictionary<string, string> parameters = null)
        {

            var baseUri = requestUri;

            if (parameters != null)
            {
                foreach(var parameter in parameters)
                {
                    baseUri += $"?{parameter.Key}={parameter.Value}";
                }
            }

            var json = await getDataAsync(baseUri);
            var datas = JObject.Parse(json).SelectToken(token).ToObject<List<Data>>();


            switch (baseUri)
            {
                case GLOBAL_DATA_URL:
                    printGlobalData(datas.First());
                    break;
                case DATA_BY_DEPARTEMENT_URL:
                    string departement = string.Empty;
                    parameters.TryGetValue("Departement", out departement);
                    printOneDepartementData(datas.First(), departement);
                    break;
                case ALL_DEPARTEMENTS_URL:
                    printDepartmentsData(datas);
                    break;
                default:
                    Console.WriteLine("Erreur durant l'exécution");
                    break;
            }

        }

        private static void printOneDepartementData(Data data, string nom)
        {
            if (data == null)
            {
                Console.WriteLine($"Aucun département n'a été trouvé avec le nom \"{nom}\"");
            }
            Console.WriteLine($"Voici les données globales connues à l'heure actuelle pour le département choisi : \"{nom}\"");


            printData(data);
        }

        static void printGlobalData(Data data)
        {
            Console.WriteLine("Voici les données globales connus pour la france");
            printData(data, global: true);
        }
        static void printDepartmentsData(List<Data> datas)
        {
            Console.WriteLine($"Voici les données globales connus pour tous les departements");
            foreach (var data in datas)
            {
                printData(data);
            }
        }

        static void printData(Data data, bool global = false)
        {
            var localisation = global ? "Pays" : "Département";
            printer.Line();
            printer.Row("Date", localisation, "Décés", "Guéris", "Hospitalisés", "Réanimation");
            printer.Line();
            printer.Row(data.date.ToString("dd/MM/yyyy"), data.nom, data.deces, data.gueris, data.hospitalises, data.reanimation);
            printer.Line();
        }


    }
}
