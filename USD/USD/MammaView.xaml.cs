using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using USD.MammaViewModels;

namespace USD
{
    /// <summary>
    ///     Interaction logic for MammaView.xaml
    /// </summary>
    public partial class MammaView : Window
    {
        public MammaView(MammaViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.Item != CollectionView.NewItemPlaceholder)
                e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof (DataGridCell))
            {
                // Starts the Edit on the row;
                var grd = (DataGrid) sender;
                grd.BeginEdit(e);
            }
        }

        private void NewPacient_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MammaViewModel;
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
                var dialogResult = MessageBox.Show("Вы уверны, что хотите начать прием нового пациента?",
                    "УЗД молочной железы",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (dialogResult == MessageBoxResult.No)
                {
                    return;
                }
            }

            DataContext = ContainerFactory.Get<MammaViewModel>();

            MainScrollViewer.ScrollToTop();
        }

        private void MammaView_OnClosing(object sender, CancelEventArgs e)
        {
            var viewModel = DataContext as MammaViewModel;
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