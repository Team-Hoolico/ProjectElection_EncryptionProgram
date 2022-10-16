using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace database
{
    public partial class Form1 : Form
    {
        int[] ClassID = new int[] { 12, 18, 14, 21, 16, 24, 45, 36, 50, 40, 55, 44, 60, 48 };

        public Form1()
        {
            InitializeComponent();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

           string[] NameSurname = textBox1.Text.Replace('i', 'ı').ToUpper().Split(' ');     //"Emine Aydogdu" = "EMINE" , "AYDOGDU"
            long UID = 0;
            int TotalNameSurname =0;
            List<int> NameSurnameNumber = new List<int> {};

            foreach (string name in NameSurname)
            {
                int Total = name[0] - 64;   //Gets first letter of the name ('E') converts it into a number (69)(hehe nice :D) and subtracts 64(5)
                for (int i = 1; i < NameSurname.Length; i++)
                {
                    Total += (name[i] - 64) * (i % 2 == 1 ? 1 : -1);
                }
                if (Total < 0)
                {
                    Total += 99;
                }

                NameSurnameNumber.Add(Total);//0th member -> NameNumber, 1st member -> SurnameNumber
            }
            NameSurnameNumber.Add(TotalNameSurname);

            string[] ClassID = comboBox1.GetItemText();




            UID += (NameSurnameNumber[1] * 1000000) + (NameSurnameNumber[0] * 10000) + (NameSurnameNumber[2] * 100); //final number appears in here
            label1.Text = $"UID = {UID}";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
