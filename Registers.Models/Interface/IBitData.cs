using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Models.Interface
{
    public interface IBitData : IBaseData
    {
        int BitIndex { get; set; }
    }
}
