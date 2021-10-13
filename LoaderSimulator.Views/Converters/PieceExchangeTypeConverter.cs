using LoaderSimulator.StateMachine.Enums;
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
    class PieceExchangeTypeConverter : ValueToOptionConverter<ExchangeType>
    {
    }

    [ContentProperty("Then")]
    class PieceExchangeTypeConverterOption : ValueOption<ExchangeType>
    {
    }
}
