using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.ViewModels.Interfaces
{
    public interface IBitObserver
    {
        bool DataChange(int register, int bit, bool value);
    }
}
