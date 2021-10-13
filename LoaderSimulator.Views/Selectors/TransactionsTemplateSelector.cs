using LoaderSimulator.ViewModels.Enums;
using Registers.Models.Interface;
using Registers.Views.Utils.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace LoaderSimulator.Views.Selectors
{
    [ContentProperty("Templates")]
    class TransactionsTemplateSelector : BaseListViewCellTemplateSelector<Transaction>
    {
        //public List<TransactionsTemplateSelectorOption> Templates { get; set; } = new List<TransactionsTemplateSelectorOption>();

        //public override DataTemplate SelectTemplate(object item, DependencyObject container)
        //{
        //    DataTemplate dt = null;

        //    if (item is IPropertyEquatable<Transaction> obj)
        //    {
        //        foreach (var t in Templates)
        //        {
        //            if (obj.PropEquals(t.When))
        //            {
        //                dt = t.Then;
        //                break;
        //            }
        //        }
        //    }

        //    return dt;
        //}
    }

    [ContentProperty("Then")]
    class TransactionsTemplateSelectorOption : BaseListViewCellTemplateSelectorOption<Transaction>
    {
        //public Transaction When { get; set; }
        //public DataTemplate Then { get; set; }
    }
}
