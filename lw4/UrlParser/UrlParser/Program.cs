using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;

namespace UrlParser
{
    class currency
    {
        public int Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }

    class Program
    {
        static List<string> typeList = new List<string>();
        static string jsonString;
        static async Task ReadValutesAcync()
        {
            using (StreamReader reader = new StreamReader("in.txt"))
            {
                string result;
                while ((result = await reader.ReadLineAsync())!=null)
                {
                    typeList.Add(result);
                    Console.WriteLine(result);
                }
            }
            Console.WriteLine($"types reading completed");
            //return typeList;
        }

        static async Task<List<currency>> GetJsonFromUrl()
        {
            string url = "https://www.cbr-xml-daily.ru/daily_json.js";
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            jsonString = await client.DownloadStringTaskAsync(new Uri(url));
            //Console.WriteLine(jsonString);
            List<currency> valuteList = new List<currency>();
            valuteList = await Task.Run(()=>ParseJsonString());
            Console.WriteLine($"json downloading completed");
            return valuteList;
        }

        static List<currency> ParseJsonString()
        {
            JObject o = JsonConvert.DeserializeObject<JObject>(jsonString);
            JObject valuteArray = o.Value<JObject>("Valute");

            List<currency> currencyList = new List<currency>();

            foreach (string valuteType in typeList)
            {
                JObject curType = valuteArray.Value<JObject>(valuteType);
                currency cc = new currency();
                cc.Nominal = curType.Value<int>("Nominal");
                cc.Value = curType.Value<double>("Value");
                cc.Name = curType.Value<string>("Name");
                currencyList.Add(cc);
            }
            Console.WriteLine($"json parsing completed");
            return currencyList;
        }

        static async Task SaveInfoAboutValutesAsync()
        {
            List<currency> valutes = GetJsonFromUrl().Result;
            using (StreamWriter sw = new StreamWriter("out.txt"))
            {
                foreach (currency valute in valutes)
                {
                    await sw.WriteLineAsync($"{valute.Nominal} {valute.Name} по курсу {valute.Value} руб.");
                }
            }
            Console.WriteLine($"info saving completed");
        }



        static void Main(string[] args)
        {
            ReadValutesAcync();
            //GetJsonStringFromUrl();
            SaveInfoAboutValutesAsync();
            Console.Read();
        }
    }
}































/*
    class currency
    {
        public int Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }

    class UrlParser
    {
        private string url = "https://www.cbr-xml-daily.ru/daily_json.js";
        
        public string GetJsonStringFromUrl()
        {
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string jsonString = client.DownloadString(url);
            return jsonString;
        }

        public List<currency> ParseJsonString(List<string> currencyTypeList)
        {
            //чтение строки по адресу
            string json = GetJsonStringFromUrl();
            JObject o = JsonConvert.DeserializeObject<JObject>(json);
            JObject valuteArray = o.Value<JObject>("Valute");

            List<currency> currencyList = new List<currency>();

            foreach (string valuteType in currencyTypeList)
            {
                JObject curType = valuteArray.Value<JObject>(valuteType);
                currency cc = new currency();
                cc.Nominal = curType.Value<int>("Nominal");
                cc.Value = curType.Value<double>("Value");
                cc.Name = curType.Value<string>("Name");
                currencyList.Add(cc);
            }
            return currencyList;
        }
        
    }

    class FileController
    {
        public string InputFileName { get; set; }
        public string OutputFileName { get; set; }

        public FileController(string inputFileName, string outputFileName)
        {
            InputFileName = inputFileName;
            OutputFileName = outputFileName;
        }

        async Task<List<string>> ReadCurrencyTypes()
        {
            List<string> currencyTypeList = new List<string>();

            if (File.Exists(InputFileName))
            {
                using (StreamReader sr = File.OpenText(InputFileName))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        currencyTypeList.Add(s);
                    }
                }
            }
            return currencyTypeList;
        }

        public void SaveInfoAboutValutes(List<currency> valuteList)
        {
            using (StreamWriter sw = new StreamWriter(OutputFileName))
            {
                foreach (currency valute in valuteList)
                {
                    sw.WriteLine($"{valute.Nominal} {valute.Name} по курсу {valute.Value} руб.");
                }
            }

        }
    }
    */
