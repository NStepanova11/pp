using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace lw4_async
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void DoButton_Click(object sender, EventArgs e)
        {
            string inFileName = inputFileName.Text;
            string outFileName = outputFileName.Text;


            if (inFileName != "" && outFileName != "")
            {
                int step = 30;
                for (int i = 0; i < step; i++)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    await AsyncVariant.Perform(inFileName, outFileName);
                    timeBox.AppendText($"{sw.ElapsedMilliseconds}  {Environment.NewLine}");
                }
            }
        }
    }
}
































/*
errLabel.Text = "";
            string inFileName = Convert.ToString(inputFileName.Text);
            string outFileName = Convert.ToString(outputFileName.Text);

            if (inFileName!="" && outFileName!="")
            {
                List<long> timeList = new List<long>();
                int step = 30;
                for (int i = 0; i < step; i++)
                {
                    AsyncVar asyncVar = new AsyncVar(); ;
                    long time = asyncVar.perform(inFileName, outFileName);
                    timeList.Add(time);
                    timeBox.AppendText($"{i + 1}. Time = {time} {Environment.NewLine}");
                }

            }
            else
            {
                errLabel.Text = "Enter the names of input/output files";
            }
 *  */
