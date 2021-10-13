using Registers.Models.Interface;
using Registers.ViewModels;
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
    public class BaseListViewCellTemplateSelector<T> : DataTemplateSelector
    {
        public List<BaseListViewCellTemplateSelectorOption<T>> Templates { get; set; } = new List<BaseListViewCellTemplateSelectorOption<T>>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate dt = null;

            if (item is IPropertyEquatable<T> obj)
            {
                foreach (var t in Templates)
                {
                    if(obj.PropEquals(t.When))
                    {
                        dt = t.Then;
                        break;
                    }
                }
            }

            return dt;
        }
    }

    public class BaseListViewCellTemplateSelectorOption<T>
    {
        public T When { get; set; }
        public DataTemplate Then { get; set; }
    }
}
