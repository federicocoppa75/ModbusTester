using LoaderSimulator.StateMachine.Enums;
using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine.Messages
{
    public class GetSignalForStartLoadMessage
    {
        /// <summary>
        /// Func per comunicare quale segnale comunica il carico, in che zona avviene il carico e se è su battuta o su cinta
        /// </summary>
        public Action<int, ExchangeType, IBitData> SetSignal { get; set; }
    }
}
