using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace lw4
{
    class currency
    {
        public int Nominal { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
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
}
