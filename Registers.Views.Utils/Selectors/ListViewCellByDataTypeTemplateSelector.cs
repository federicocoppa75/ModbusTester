using Registers.ViewModels;
using Registers.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Registers.Views.Utils.Selectors
{
    [ContentProperty("Templates")]
    public class ListViewCellByDataTypeTemplateSelector : BaseListViewCellTemplateSelector<DataType>
    {
    }

    [ContentProperty("Then")]
    public class ListViewCellByDataTypeTemplateSelectorOption : BaseListViewCellTemplateSelectorOption<DataType>
    {
    }
}
