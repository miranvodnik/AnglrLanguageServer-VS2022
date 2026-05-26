using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AnglrLangExtension
{
    internal class RhsNodeSetToStringConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture) =>
            (parameter is Int32) ? "Rhs Node Set " + (Int32) parameter : "Rhs Node Set " + parameter.ToString();

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
