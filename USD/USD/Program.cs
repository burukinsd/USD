using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using SimpleInjector;
using USD.DAL;
using USD.MammaViewModels;
using USD.Properties;

namespace USD
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            DbMigration();

            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.
            ContainerFactory.Initialize(container);
            RunApplication(container);
        }

        private static void DbMigration()
        {
            using (var db = new LiteDatabase(Settings.Default.LiteDbFileName))
            {
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
                //Log the exception and exit
            }
        }
    }
}