using LoaderSimulator.StateMachine.Enums;
using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine.Messages
{
    public class GetSignalForStartUnloadMessage
    {
        /// <summary>
        /// Func per comunicare quale segnale comunica lo scarico, in che zona avviene il carico e se è su battuta o sulla pinza
        /// </summary>
        public Action<int, ExchangeType, IBitData> SetSignal { get; set; }
    }
}
