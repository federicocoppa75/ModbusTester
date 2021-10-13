using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.StateMachine.Messages;
using Registers.Models.Interface;

namespace LoaderSimulator.StateMachine
{
    public abstract class LoadingState : PieceExchangingState
    {
        enum InternalState
        {
            WaitForLoadRequest,
            WaitForPhotocellSignal,
            WaitForPanelTaken,
            ClosingTransaction,
            ClosedTransaction
        }

        InternalState _internalState = InternalState.WaitForLoadRequest;

        IBitData _scmPhotocell;
        IBitData _exNoInterference;
        IBitData _scmLoadReq;
        IBitData _exPieceReady;
        IBitData _scmPieceTaken;
        IBitData _exLoadEnd;

        public override ExchangeDirection ExchangeDirection =>  ExchangeDirection.Load;

        public override string Name => "Loading State";

        public LoadingState() : base()
        {

        }

        public override void Start()
        {
            base.Start();

            if(GetValue(_scmLoadReq))
            {
                StartLoading();
            }
            else
            {
                Context.PreLoadPanel(PanelExchangeZone, ExchangeType);
            }
        }

        public override void Reset()
        {
            base.Reset();

            SetValue(_exPieceReady, false);
            SetValue(_exNoInterference, true);
            SetValue(_exLoadEnd, false);
        }

        #region initialize

        protected override void RegisterSignalObserver()
        {
            base.RegisterSignalObserver();

            RegisterSignalObserver(_scmPhotocell);
            RegisterSignalObserver(_exNoInterference);
            RegisterSignalObserver(_scmLoadReq);
            RegisterSignalObserver(_exPieceReady);
            RegisterSignalObserver(_scmPieceTaken);
            RegisterSignalObserver(_exLoadEnd);
        }

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


        private IEnumerable<Signals> GetSignalRequestList()
        {
            var signals = new Signals[]
            {
                Signals.SCM_PHOTOCELL,
                Signals.EX_NO_INTERFERENCE,
                Signals.SCM_LOAD_REQ,
                Signals.EX_PIECE_READY,
                Signals.SCM_PIECE_TAKEN,
                Signals.EX_LOAD_END
            };

            return signals;
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
                case Signals.SCM_LOAD_REQ:
                    _scmLoadReq = signal;
                    break;
                case Signals.EX_PIECE_READY:
                    _exPieceReady = signal;
                    break;
                case Signals.SCM_PIECE_TAKEN:
                    _scmPieceTaken = signal;
                    break;
                case Signals.EX_LOAD_END:
                    _exLoadEnd = signal;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region state machine

        public override bool DataChange(int register, int bit, bool value)
        {
            bool result = base.DataChange(register, bit, value);

            if(!result)
            {
                switch (_internalState)
                {
                    case InternalState.WaitForLoadRequest:
                        if(value && IsSameSignal(_scmLoadReq, register, bit)) StartLoading();
                        break;
                    case InternalState.WaitForPhotocellSignal:
                        if (value && IsSameSignal(_scmPhotocell, register, bit)) RequestLeavePanelInMachine();
                        break;
                    case InternalState.WaitForPanelTaken:
                        if (value && IsSameSignal(_scmPieceTaken, register, bit)) BeginCloseTransaction();
                        break;
                    case InternalState.ClosingTransaction:
                        if (!value && IsSameSignal(_scmPieceTaken, register, bit)) EndCloseTransaction();
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        private void StartLoading()
        {
            _internalState = InternalState.WaitForPhotocellSignal;

            SetValue(_exNoInterference, false);

            Context.LoadPanel(PanelExchangeZone, ExchangeType);
        }

        private void RequestLeavePanelInMachine()
        {
            _internalState = InternalState.WaitForPanelTaken;

            Context.RequestLoadPanelExcutionConferm(PanelExchangeZone, ExchangeType, () => SetValue(_exPieceReady, true));
        }

        private void BeginCloseTransaction()
        {
            _internalState = InternalState.ClosingTransaction;

            SetValue(_exPieceReady, false);
            SetValue(_exNoInterference, true);
            SetValue(_exLoadEnd, true);
        }

        private void EndCloseTransaction()
        {
            _internalState = InternalState.ClosedTransaction;

            SetValue(_exLoadEnd, false);

            Reset();
            //Context.State = new WaitForUnloadSelectionState() { Context = Context };
            Context.State = new IdleState() { Context = Context };
            Context.State.Start();
            Context = null;
        }

        #endregion

    }
}
