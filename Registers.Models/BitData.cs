using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Models
{
    public class BitData : BaseData, IBitData, IBaseData
    {
        public int BitIndex { get; set; }
    }
}
