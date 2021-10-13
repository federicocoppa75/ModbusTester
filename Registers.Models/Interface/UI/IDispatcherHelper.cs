using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Models.Interface.UI
{
    public interface IDispatcherHelper
    {
        void CheckBeginInvokeOnUi(Action action);
    }
}
