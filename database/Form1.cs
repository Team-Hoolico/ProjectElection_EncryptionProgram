using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace database
{
    public partial class Form1 : Form
    {
        int[] ClassID = new int[] { 12, 18, 14, 21, 16, 24, 45, 36, 50, 40, 55, 44, 60, 48};

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
            int TotalNameSurname = 0;
            List<int> NameSurnameNumber = new List<int>();

            foreach (string name in NameSurname)
            {
                int Total = name[0] - 64;   //Gets first letter of the name ('E') converts it into a number (69)(hehe nice :D) and subtracts 64(5)
                for (int i = 1; i < name.Length; i++)
                {
                    Total += (name[i] - 64) * (i % 2 == 1 ? 1 : -1);
                }
                if (Total < 0){
                    Total += 99;
                }

                TotalNameSurname += Total;
                NameSurnameNumber.Add(Total);//0th member -> NameNumber, 1st member -> SurnameNumber
            }

            if (TotalNameSurname > 99){
                TotalNameSurname -= 99;
            }

            NameSurnameNumber.Add(TotalNameSurname);

            UID += (NameSurnameNumber[1] * 1000000) + (NameSurnameNumber[0] * 10000) + (NameSurnameNumber[2] * 100) + (ClassID[comboBox1.SelectedIndex]); //final number appears in here

            //We log newly generated UID's for in case of fraud
            LogUID(UID);
        }

        private async void LogUID(long uid){
            // Putting UID's in DB after generating is a good way to ensure no one can make random passwords ehehe >:3
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(JsonSerializer.Serialize(new{UID = uid}));
            client.BaseAddress = new Uri("https://localhost:7241/");
            
            using (StreamWriter writer = new StreamWriter(@"Log.txt", true)){
                try{
                    // Try to register
                    HttpResponseMessage response = await client.PostAsync($"/RegisterVoter/?UID={uid}", new StringContent("",Encoding.UTF8,"application/json"));
                    if (!response.IsSuccessStatusCode) {
                        //Failed :(
                        throw new Exception($"API failed! {await response.Content.ReadAsStringAsync()}");
                    }
                    // Worked!
                    writer.WriteLine($"[{DateTime.Now}] Generated key '{uid}' has been registered to database");
                    label1.Text = $"UID = {uid}";
                }catch(Exception e){
                    // Might need to shorten the exceptions 
                    writer.WriteLine($"[{DateTime.Now}] Generated key '{uid}' COULDN'T BE REGISTERED TO DATABASE!. "+e.Message);
                    label1.Text = $"UID = There has been an error with UID check logs";
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
