using Registers.Models.Interface;
using Registers.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registers.ViewModels.Utils.Factories
{
    public class ClockDataViewModelFactory : IClockDataViewModelFactory
    {
        public IClockData Create() => new ClockDataViewModel();
    }
}
