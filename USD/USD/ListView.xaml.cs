﻿using System.IO;
using System.Linq;
using System.Windows;
using LiteDB;
using Microsoft.Win32;
using USD.Properties;

namespace USD
{
    /// <summary>
    ///     Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : Window
    {
        public ListView(ListViewModel.ListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void Import_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Файлы БД программы (USD.db)|USD.db|Все файлы БД (*.db)|*.db|Все файлы (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                using (var db = new LiteDatabase(DirectoryHelper.GetDataDirectory() + Settings.Default.LiteDbFileName))
                {
                    using (var db1 = new LiteDatabase(openFileDialog.FileName))
                    {
                        if (!db1.CollectionExists("screenings"))
                        {
                            MessageBox.Show(
                                "Не подходящая база данных. Используйте базу данных, только от этой программы.", "УЗД",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var origCol = db.GetCollection("screenings");
                        var newCol = db1.GetCollection("screenings");
                        foreach (var source in newCol.FindAll().ToList())
                        {
                            source["Id"] = null;
                            origCol.Insert(source);
                        }
                    }
                }
                (DataContext as ListViewModel.ListViewModel)?.LoadData();
                MessageBox.Show("Данные успешно импортированны", "УЗД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog {Filter = "Файлы БД (*.db)|*.db"};
            if (saveFileDialog.ShowDialog() == true)
            {
                File.Copy(DirectoryHelper.GetDataDirectory() + Settings.Default.LiteDbFileName, saveFileDialog.FileName);
                MessageBox.Show("База успешно экспортирована.", "УЗД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}