using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Models
{
    public class ClockData : BitData, IClockData, IBitData, IBaseData
    {
        public int Period { get; set; }
    }
}
