using System;
using SimpleInjector;
using USD.DAL;
using USD.MammaViewModels;

namespace USD
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.

            RunApplication(container);
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