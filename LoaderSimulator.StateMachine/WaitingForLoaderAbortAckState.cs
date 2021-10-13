using LoaderSimulator.StateMachine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine
{
    public class WaitingForLoaderAbortAckState : WaitingForAbortAckState
    {
        public override string Name => $"Abort from loader";

        enum InternalState
        {
            WaitForAck,
            CloseTransaction
        }

        InternalState _internalState = InternalState.WaitForAck;

        protected override Signals AbortSignalId => Signals.EX_ABORT_REQ;
        protected override Signals AckSignalId => Signals.SCM_ABORT_ACK;
        public WaitingForLoaderAbortAckState() : base()
        {

        }
        
        public override bool DataChange(int register, int bit, bool value)
        {
            if(IsActive)
            {
                switch (_internalState)
                {
                    case InternalState.WaitForAck:
                        if (IsSameSignal(AckSignal, register, bit)) CloseTransaction();
                        break;
                    case InternalState.CloseTransaction:
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        private void CloseTransaction()
        {
            _internalState = InternalState.CloseTransaction;

            SetValue(AbortSignal, false);

            Reset();
            Context.State = new IdleState() { Context = Context };
            Context.State.Start();
            Context = null;
        }
    }
}
