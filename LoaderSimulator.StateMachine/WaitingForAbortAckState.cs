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
    public abstract class WaitingForAbortAckState : BaseState, IBitObserver
    {
        protected IBitData AbortSignal { get; private set; }
        protected IBitData AckSignal { get; private set; }

        protected abstract Signals AbortSignalId { get; }
        protected abstract Signals AckSignalId { get; }

        public WaitingForAbortAckState() : base()
        {

        }

        public abstract bool DataChange(int register, int bit, bool value);

        public override void Reset()
        {
            IsActive = false;
        }

        public override void Start()
        {
            Messenger.Default.Send(new GetSignalMessage() { Signal = AbortSignalId, SetSignal = (s, bd) => AbortSignal = bd });
            Messenger.Default.Send(new GetSignalMessage() { Signal = AckSignalId, SetSignal = (s, bd) => AckSignal = bd });

            IsActive = true;

            RegisterSignalObserver(AbortSignal);
            RegisterSignalObserver(AckSignal);
        }

        protected void RegisterSignalObserver(IBitData signal) => RegisterSignalObserver(signal, this);
    }
}
