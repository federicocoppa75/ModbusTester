using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Registers.Views.Utils.Converters
{
    public class ValueToOptionConverter<T> : IValueConverter //where T : IEquatable<T>
    {
        public List<ValueOption<T>> Options { get; set; } = new List<ValueOption<T>>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value;

            if(value is T v)
            {
                foreach (var item in Options)
                {
                    if(EqualityComparer<T>.Default.Equals(v, item.When))
                    {
                        result = item.Then;
                        break;
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ValueOption<T>
    {
        public T When { get; set; }
        public object Then { get; set; }
    }
}
