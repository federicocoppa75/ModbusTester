using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.StateMachine.Messages;
using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine
{
    public class UnloadingOnBeltState : UnloadingState
    {
        enum InternalState
        {
            WaitForStartRequest,
            WaitForPhotocellSignal,
            ClosingTransaction,
            ClosedTransaction
        }

        InternalState _internalState = InternalState.WaitForStartRequest;

        IBitData _scmPhotocell;
        IBitData _scmUnloadReq;
        IBitData _exUnloadEnd;
        IBitData _exNoInterference;

        public override ExchangeType ExchangeType => ExchangeType.OnBelt;
        public override string Name => ((_scmUnloadReq != null) && GetValue(_scmUnloadReq)) ? "Unloading state" : "Preunloading state";

        public UnloadingOnBeltState() : base()
        {

        }

        public override void Start()
        {
            base.Start();

            if(GetValue(_scmUnloadReq))
            {
                StartUnload();
            }
            else
            {
                Context.PreUnloadPanel(PanelExchangeZone, ExchangeType);
            }
        }

        public override void Reset()
        {
            base.Reset();

            SetValue(_exNoInterference, true);
        }

        #region initialize

        protected override void RequestSignalToListen()
        {
            base.RequestSignalToListen();

            Messenger.Default.Send(new GetSignalsForPieceExchangeMessage()
            {
                Position = PanelExchangeZone,
                ExchangeDirection = ExchangeDirection,
                ExchangeType = ExchangeType,
                SignalsRequest = GetSignalRequestList(),
                SetSignal = SetSignal
            });
        }

        private void SetSignal(Signals signalId, IBitData signal)
        {
            switch (signalId)
            {
                case Signals.SCM_PHOTOCELL:
                    _scmPhotocell = signal;
                    break;
                case Signals.EX_NO_INTERFERENCE:
                    _exNoInterference = signal;
                    break;
                case Signals.SCM_UNLOAD_REQ:
                    _scmUnloadReq = signal;
                    break;
                case Signals.EX_UNLOAD_END:
                    _exUnloadEnd = signal;
                    break;
                default:
                    break;
            }
        }

        private IEnumerable<Signals> GetSignalRequestList()
        {
            var signals = new Signals[]
            {
                Signals.SCM_PHOTOCELL,
                Signals.SCM_UNLOAD_REQ,
                Signals.EX_NO_INTERFERENCE,
                Signals.EX_UNLOAD_END
            };

            return signals;
        }

        protected override void RegisterSignalObserver()
        {
            base.RegisterSignalObserver();

            RegisterSignalObserver(_scmPhotocell);
            RegisterSignalObserver(_scmUnloadReq);
            RegisterSignalObserver(_exUnloadEnd);
            RegisterSignalObserver(_exNoInterference);
        }

        #endregion

        #region state machine

        public override bool DataChange(int register, int bit, bool value)
        {
            bool result =  base.DataChange(register, bit, value);

            if(!result)
            {
                switch (_internalState)
                {
                    case InternalState.WaitForStartRequest:
                        if (IsSameSignal(_scmUnloadReq, register, bit) && value) StartUnload();
                        break;
                    case InternalState.WaitForPhotocellSignal:
                        if (IsSameSignal(_scmPhotocell, register, bit) && !value) BeginCloseTransaction();
                        break;
                    case InternalState.ClosingTransaction:
                        if (IsSameSignal(_scmUnloadReq, register, bit) && !value) EndCloseTransaction();
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        private void StartUnload()
        {
            _internalState = InternalState.WaitForPhotocellSignal;

            SetValue(_exNoInterference, false);

            Context.UnloadPanel(PanelExchangeZone, ExchangeType);
        }

        private void BeginCloseTransaction()
        {
            _internalState = InternalState.ClosingTransaction;

            SetValue(_exNoInterference, true);
            SetValue(_exUnloadEnd, true);
        }

        private void EndCloseTransaction()
        {
            _internalState = InternalState.ClosingTransaction;

            SetValue(_exUnloadEnd, false);

            Reset();
            Context.State = new IdleState() { Context = Context };
            Context.State.Start();
            Context = null;
        }

        #endregion
    }
}
