using LoaderSimulator.StateMachine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine
{
    public abstract class UnloadingState : PieceExchangingState
    {
        //public override string Name => $"Unloading state";
        public override ExchangeDirection ExchangeDirection => ExchangeDirection.Unload;

        public UnloadingState() : base()
        {

        }

    }
}
