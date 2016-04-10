using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace USD.ViewTools
{
    public class RowToIndexConverter : MarkupExtension, IValueConverter
    {
        private static RowToIndexConverter converter;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var row = value as DataGridRow;
            return row?.GetIndex() + 1 ?? -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new RowToIndexConverter());
        }
    }
}