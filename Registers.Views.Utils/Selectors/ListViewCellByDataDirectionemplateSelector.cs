using Registers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Registers.Views.Utils.Selectors
{
    [ContentProperty("Templates")]
    public class ListViewCellByDataDirectionemplateSelector : BaseListViewCellTemplateSelector<DataDirection>
    {
    }

    [ContentProperty("Then")]
    public class ListViewCellByDataDirectionemplateSelectorOption : BaseListViewCellTemplateSelectorOption<DataDirection>
    {
    }
}
