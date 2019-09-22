using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace UrlParser
{
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

        public List<string> ReadCurrencyTypes()
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

    class Program
    {
        static void Main(string[] args)
        {
            UrlParser parser = new UrlParser();
            List<string> valuteTypeList = new List<string>();
            List<currency> valuteList = new List<currency>();

            FileController fController = new FileController("in.txt", "out.txt");
            valuteTypeList = fController.ReadCurrencyTypes();

            valuteList = parser.ParseJsonString(valuteTypeList);

            fController.SaveInfoAboutValutes(valuteList);

            /*
                Console.WriteLine(cc.Nominal.ToString());
                Console.WriteLine(cc.Value.ToString());
                Console.WriteLine(cc.Name.ToString());
            */
        }
    }
}
