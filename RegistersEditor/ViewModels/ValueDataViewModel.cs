using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistersEditor.ViewModels
{
    public class ValueDataViewModel : BaseDataViewModel, IValueData, IBaseData
    {
        [Browsable(false)]
        public string BitIndex => "--";

        public ValueDataViewModel() : base()
        {

        }
    }
}
