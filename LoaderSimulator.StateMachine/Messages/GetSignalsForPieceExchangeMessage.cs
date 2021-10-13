using LoaderSimulator.StateMachine.Enums;
using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine.Messages
{
    public class GetSignalsForPieceExchangeMessage
    {
        public int Position { get; set; }
        public ExchangeDirection ExchangeDirection { get; set; }
        public ExchangeType ExchangeType { get; set; }
        public IEnumerable<Signals> SignalsRequest { get; set; }
        public Action<Signals, IBitData> SetSignal { get; set; }
    }
}
