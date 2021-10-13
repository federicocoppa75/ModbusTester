using Registers.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.ViewModels.Messages
{
    public class StartClockMessage
    {
        public DataDirection Direction { get; set; }
    }
}
