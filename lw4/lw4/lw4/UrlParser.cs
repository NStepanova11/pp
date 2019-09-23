using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace lw4
{
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
}
