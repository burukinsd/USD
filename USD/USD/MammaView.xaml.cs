using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace USD
{
    /// <summary>
    /// Interaction logic for MammaView.xaml
    /// </summary>
    public partial class MammaView : Window
    {
        public MammaView(MammaViewModels.MammaViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.Item != CollectionView.NewItemPlaceholder)
                e.Row.Header = (e.Row.GetIndex()+1).ToString();
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);
            }
        }

        private void NewPacient_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MammaViewModels.MammaViewModel;
            if (viewModel != null && viewModel.SaveCommand.CanExecute(null))
            {
                var dialogResult = MessageBox.Show("Есть несохраненные изменения. Сохранить их?", "УЗД молочной железы",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (dialogResult)
                {
                    case MessageBoxResult.Yes:
                        viewModel.SaveCommand.Execute(null);
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            else
            {
                var dialogResult = MessageBox.Show("Вы уверны, что хотите начать прием нового пациента?", "УЗД молочной железы",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (dialogResult == MessageBoxResult.No)
                {
                    return;
                }
            }

            DataContext = ContainerFactory.Get<MammaViewModels.MammaViewModel>();
        }

        private void MammaView_OnClosing(object sender, CancelEventArgs e)
        {
            var viewModel = DataContext as MammaViewModels.MammaViewModel;
            if (viewModel != null && viewModel.SaveCommand.CanExecute(null))
            {
                var dialogResult = MessageBox.Show("Есть несохраненные изменения. Сохранить их?", "УЗД молочной железы",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes);
                switch (dialogResult)
                {
                    case MessageBoxResult.Yes:
                        viewModel.SaveCommand.Execute(null);
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
            else
            {
                var dialogResult = MessageBox.Show("Вы уверны, что хотите выйти?", "УЗД молочной железы",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (dialogResult == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
