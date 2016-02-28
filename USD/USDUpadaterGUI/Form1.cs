using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USDUpadaterGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var checkVersionBW = new BackgroundWorker();
            checkVersionBW.DoWork += new DoWorkEventHandler(GetUpdateVersion); 
        }




        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 100;
            textBox1.Text = @"Начата проверка обновлений...";
            var currentVersion = Assembly.LoadFile($"{Directory.GetCurrentDirectory()}\\USD.exe").GetName().Version;
            textBox1.Text += $"Установленная версия: {currentVersion}";
            
            var updateVersion = Version.Parse(.Result);
            textBox1.Text += $"Версия на сервере: {updateVersion}";
            if (updateVersion > currentVersion)
            {
                textBox1.Text += @"Доступно обновление.";
                var res = MessageBox.Show("Достпно обновление. Обновить сейчас?", "Обновление УЗД", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                button2.Enabled = true;
                if (res == DialogResult.Yes)
                {
                    
                }
            }
            else
            {
                textBox1.Text += @"Нет доступных обновлений.";
            }
        }

        private void GetUpdateVersion(object sender, DoWorkEventArgs args)
        {
            string responseString = String.Empty;
            using (var client = new HttpClient())
            {
                try
                {
                    var responseTask = client.GetAsync("http://usdupdate.azurewebsites.net/install/version.htm");
                    responseTask.Wait();
                    var responseStringTask = responseTask.Result.Content.ReadAsStringAsync();
                    responseStringTask.Wait();
                    args.Result = responseStringTask.Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Не удается получить версию обновления.");
                }
            }
        }
    }
}
