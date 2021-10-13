using Registers.Views.Utils.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace LoaderSimulator.Views.Converters
{
    [ContentProperty("Options")]
    class PieceExchangePositionConverter : ValueToOptionConverter<int>
    {
    }

    [ContentProperty("Then")]
    class PieceExchangePositionConverterOption : ValueOption<int>
    {
    }
}
