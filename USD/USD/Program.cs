using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using LiteDB;
using Microsoft.Win32;
using Newtonsoft.Json;
using SimpleInjector;
using USD.DAL;
using USD.MammaViewModels;
using USD.Properties;
using USD.WordExport;

namespace USD
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            EnsureDbFile();
            DbMigration();

            ExportDirectoryCreator.EnsureDirectory();

            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.
            ContainerFactory.Initialize(container);
            RunApplication(container);
        }

        private static void EnsureDbFile()
        {
            var specialDirectory = DirectoryHelper.GetDataDirectory();
            if (!File.Exists($"{specialDirectory}{Settings.Default.LiteDbFileName}"))
            {
                var dialogResult =
                    MessageBox.Show(
                        "База данных не найдена. Вы хотите указать имеющийся файл базы дынных? Иначе будет создана новая база.",
                        "УЗД", MessageBoxButton.YesNo,
                        MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (dialogResult)
                {
                    case MessageBoxResult.Yes:
                        var openFileDialog = new OpenFileDialog
                        {
                            Filter = "Файлы БД программы (USD.db)|USD.db|Все файлы БД (*.db)|*.db|Все файлы (*.*)|*.*"
                        };
                        if (openFileDialog.ShowDialog() == true)
                        {
                            File.Copy(openFileDialog.FileName, $"{specialDirectory}{Settings.Default.LiteDbFileName}");
                            MessageBox.Show("Файл база данных успешно скопирован.", "УЗД", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        break;
                    case MessageBoxResult.No:
                        var f = File.Create(specialDirectory + Settings.Default.LiteDbFileName);
                        f.Flush();
                        f.Close();
                        break;
                }
            }
        }

        private static void DbMigration()
        {
            using (var db = new LiteDatabase(DirectoryHelper.GetDataDirectory() + Settings.Default.LiteDbFileName))
            {
                if (!db.CollectionExists("screenings")) return;
                
                var col = db.GetCollection("screenings");
                IEnumerable<BsonDocument> items = col.FindAll().ToList();
                foreach (var item in items)
                {
                    var isNeedUpdate = false;
                    var formations = item["FocalFormations"].AsArray;
                    foreach (var form in formations)
                    {
                        var size = form.AsDocument["Size"];
                        if (!size.IsString)
                        {
                            form.AsDocument.Set("Size", size.AsString);
                            isNeedUpdate = true;
                        }
                        if (size.IsNull)
                        {
                            form.AsDocument.Set("Size", String.Empty);
                            isNeedUpdate = true;
                        }
                    }

                    if (isNeedUpdate)
                    {
                        col.Update(item);
                    }
                }
            }
        }

        private static Container Bootstrap()
        {
            var container = new Container();
            container.Register<IDbWraper, LiteDbWraper>();
            container.Register<IMammaRepository, MammaRepository>();

            container.Register<MammaView>();
            container.Register<ListView>();
            container.Register<MammaViewModel>();
            container.Register<ListViewModel.ListViewModel>();

            container.Verify();

            return container;
        }

        private static void RunApplication(Container container)
        {
            try
            {
                var app = new App();
                var mainWindow = container.GetInstance<MammaView>();
                app.Run(mainWindow);
            }
            catch (Exception ex)
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("usd@burukinsd.ru", "УЗД ошибка"),
                    Subject = "Ошибка в программе УЗИ.",
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    IsBodyHtml = false,
                    Body = JsonConvert.SerializeObject(ex),
                    Priority = MailPriority.High
                };
                mail.Attachments.Add(new Attachment(DirectoryHelper.GetDataDirectory() + Settings.Default.LiteDbFileName));
                mail.To.Add("burukinsd@gmail.com");
                using (SmtpClient client = new SmtpClient()
                {
                    Host = "smtp.yandex.ru",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("usd@burukinsd.ru", "191613")
                })
                {
                    client.Send(mail);
                }
                throw;
            }
        }
    }
}
