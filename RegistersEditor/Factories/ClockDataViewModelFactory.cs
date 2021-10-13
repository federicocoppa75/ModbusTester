using Registers.Models.Interface;
using Registers.Utils.Interfaces;
using RegistersEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistersEditor.Factories
{
    class ClockDataViewModelFactory : IClockDataViewModelFactory
    {
        public IClockData Create() => new ClockDataViewModel();
    }
}
