using LoaderSimulator.StateMachine.Enums;
using Registers.Views.Utils.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace LoaderSimulator.Views.Converters
{
    [ContentProperty("Options")]
    class PieceExchangeDirectionConverter : ValueToOptionConverter<ExchangeDirection>
    {
    }

    [ContentProperty("Then")]
    class PieceExchangeDirectionConverterOption : ValueOption<ExchangeDirection>
    {
    }
}
