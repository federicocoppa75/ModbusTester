using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.StateMachine.Messages;
using Registers.Models.Interface;
using Registers.ViewModels.Interfaces;
using Registers.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine
{
    public class WaitingForMachineAbortAckState : WaitingForAbortAckState
    {
        public override string Name => $"Abort from machine";

        enum InternalState
        {
            WaitToSendAck,
            WaitForAbortReset,
            ClosedTransaction
        }

        InternalState _internalState = InternalState.WaitToSendAck;

        protected override Signals AbortSignalId => Signals.SCM_ABORT_REQ;
        protected override Signals AckSignalId => Signals.EX_ABORT_ACK;

        public WaitingForMachineAbortAckState() : base()
        {
        }

        public override void Start()
        {
            base.Start();

            _internalState = InternalState.WaitForAbortReset;

            Context.RequestMachineAbortAck(() => SetValue(AckSignal, true));
        }

        public override bool DataChange(int register, int bit, bool value)
        {
            if(IsActive)
            {
                switch (_internalState)
                {
                    case InternalState.WaitToSendAck:
                        break;
                    case InternalState.WaitForAbortReset:
                        if (IsSameSignal(AbortSignal, register, bit) && !value) CloseTransaction();
                        break;
                    case InternalState.ClosedTransaction:
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        private void CloseTransaction()
        {
            _internalState = InternalState.ClosedTransaction;

            SetValue(AckSignal, false);

            Reset();
            Context.State = new IdleState() { Context = Context };
            Context.State.Start();
            Context = null;
        }
    }
}
