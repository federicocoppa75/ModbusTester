using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Models.Interface
{
    public interface IClockData : IBitData
    {
        int Period { get; set; }
    }
}
