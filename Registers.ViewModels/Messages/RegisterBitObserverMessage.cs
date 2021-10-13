using Registers.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.ViewModels.Messages
{
    public class RegisterBitObserverMessage
    {
        public int Register { get; set; }
        public int BitIndex { get; set; }
        public IBitObserver Observer { get; set; }
    }
}
