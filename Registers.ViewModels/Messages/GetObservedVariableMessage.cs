using Registers.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.ViewModels.Messages
{
    public class GetObservedVariableMessage
    {
        public Action<int, DataType> Declare { get; set; }
    }
}
