using LoaderSimulator.StateMachine.Enums;
using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine.Messages
{
    public class GetSignalMessage
    {
        public Signals Signal { get; set; }
        public Action<Signals, IBitData> SetSignal { get; set; }
    }
}
