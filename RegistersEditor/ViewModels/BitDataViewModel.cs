using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistersEditor.ViewModels
{
    public class BitDataViewModel : BaseDataViewModel, IBitData, IBaseData
    {
        private int _bitIndex;

        public int BitIndex
        {
            get => _bitIndex; 
            set => Set(ref _bitIndex, value, nameof(BitIndex));
        }


        public BitDataViewModel() : base()
        {

        }
    }
}
