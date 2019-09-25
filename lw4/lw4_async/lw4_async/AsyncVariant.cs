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
using System.Diagnostics;

namespace lw4_async
{
    class currency
    {
        public int Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }


    class AsyncVariant
    {
        //асинхронное чтение типов валют из файла
        public static async Task<List<string>> ReadValutesAcync(string inFileName)
        {
            List<string> typeList = new List<string>();

            using (StreamReader reader = new StreamReader(inFileName))
            {
                string result;
                while ((result = await reader.ReadLineAsync()) != null)
                {
                    typeList.Add(result);
                }
            }
            return typeList;
        }


        //асинхронная загрузка json строки валют
        public static async Task<string> DonnloadJsonStringAsync()
        {
            string url = "https://www.cbr-xml-daily.ru/daily_json.js";

            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            string jsonString = await webClient.DownloadStringTaskAsync(url);
            return jsonString;
        }

        public static async Task<List<currency>> GetInfoAboutValutesAsync(List<string> typeList)
        {
            string jsonString = await DonnloadJsonStringAsync();

            JObject o = JsonConvert.DeserializeObject<JObject>(jsonString);
            JObject valuteArray = o.Value<JObject>("Valute");

            List<currency> valutes = new List<currency>();

            foreach (string valuteType in typeList)
            {
                JObject curType = valuteArray.Value<JObject>(valuteType);
                currency cc = new currency();
                cc.Nominal = curType.Value<int>("Nominal");
                cc.Value = curType.Value<double>("Value");
                cc.Name = curType.Value<string>("Name");
                valutes.Add(cc);
            }

            return valutes;
        }

        //асинхронная запись валют в файл
        public static async Task SaveInfoAboutValutesAsync(string outFileName, List<currency> valutes)
        {
            using (StreamWriter sw = new StreamWriter(outFileName))
            {
                foreach (currency valute in valutes)
                {
                    await sw.WriteLineAsync($"{valute.Nominal} {valute.Name} по курсу {valute.Value} руб.");
                }
            }
        }

        public static async Task Perform(string inFileName, string outFileName)
        {
            List<string> valuteTypes = await ReadValutesAcync(inFileName);
            List<currency> valutes = await GetInfoAboutValutesAsync(valuteTypes);
            await SaveInfoAboutValutesAsync(outFileName, valutes);
        }
    }
}
