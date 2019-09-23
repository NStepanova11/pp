using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lw4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PerformButton_Click(object sender, EventArgs e)
        {
            errLabel.Text = "";

            string inFileName = Convert.ToString(inputFileName.Text);
            string outFileName = Convert.ToString(outputFileName.Text);

            if (inFileName.Length==0 || outFileName.Length==0)
            {
                errLabel.Text = "Еnter the names of the input and output files";
            }
            else
            {
                UrlParser parser = new UrlParser();
                List<string> valuteTypeList = new List<string>();
                List<currency> valuteList = new List<currency>();

                FileController fController = new FileController(inFileName, outFileName);
                valuteTypeList = fController.ReadCurrencyTypes();

                valuteList = parser.ParseJsonString(valuteTypeList);

                fController.SaveInfoAboutValutes(valuteList);
            }
        }
    }
}
