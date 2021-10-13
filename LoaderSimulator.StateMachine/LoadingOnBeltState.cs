using LoaderSimulator.StateMachine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine
{
    public class LoadingOnBeltState : LoadingState
    {
        public override ExchangeType ExchangeType => ExchangeType.OnBelt;

        public LoadingOnBeltState(): base()
        {

        }
    }
}
