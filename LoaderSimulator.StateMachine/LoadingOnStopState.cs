using LoaderSimulator.StateMachine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine
{
    public class LoadingOnStopState : LoadingState
    {
        public override ExchangeType ExchangeType => ExchangeType.OnStop;

        public LoadingOnStopState() : base()
        {

        }
    }
}
