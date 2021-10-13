using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Utils.Interfaces
{
    public interface IClockDataViewModelFactory
    {
        IClockData Create();
    }
}
