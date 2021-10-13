using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistersEditor.ViewModels
{
    class ClockDataViewModel : BitDataViewModel, IClockData, IBitData, IBaseData
    {
        private int _period;
        public int Period
        {
            get => _period;
            set => Set(ref _period, value, nameof(Period));
        }

        public ClockDataViewModel() : base()
        {

        }
    }
}
