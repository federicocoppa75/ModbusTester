using LoaderSimulator.StateMachine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine
{
    public abstract class PieceExchangingState : ActiveConnectionState
    {
        public int PanelExchangeZone { get; set; }
        public abstract ExchangeDirection ExchangeDirection { get; }
        public abstract ExchangeType ExchangeType { get; }

        public PieceExchangingState() : base()
        {

        }
    }
}
