using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.StateMachine.Messages;
using LoaderSimulator.StateMachine.Utils;
using Registers.Comunication.Messages.ComState;
using Registers.Models.Interface;
using Registers.ViewModels.Interfaces;
using Registers.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine
{
    public class IdleState : ActiveConnectionState, IBitObserver
    {
        class SignalData
        {
            public int Position { get; set; }
            public ExchangeDirection ExchangeDirection { get; set; }
            public ExchangeType ExchangeType { get; set; }
            public IBitData Signal { get; set; }
        }

        Dictionary<Tuple<int, int>, SignalData> _signalMap = new Dictionary<Tuple<int, int>, SignalData>();

        public override string Name => "Idle state";

        public IdleState() : base()
        {            
        }

        public override void Start()
        {
            base.Start();

            CheckForActiveSignal();
        }

        public override void Reset()
        {
            base.Reset();

            _signalMap.Clear();
        }

        #region Initialize

        protected override void RequestSignalToListen()
        {
            base.RequestSignalToListen();

            Messenger.Default.Send(new GetSignalForStartLoadMessage() { SetSignal = AddLoadSignal });
            Messenger.Default.Send(new GetSignalForStartUnloadMessage() { SetSignal = AddUnloadSignal });
        }

        protected override void RegisterSignalObserver()
        {
            base.RegisterSignalObserver();

            foreach (var item in _signalMap.Keys)
            {
                Messenger.Default.Send(new RegisterBitObserverMessage() { Register = item.Item1, BitIndex = item.Item2, Observer = this });
            }
        }

        private void AddLoadSignal(int loadPlace, ExchangeType exchageType, IBitData signal)
        {
            _signalMap.Add(new Tuple<int, int>(signal.Register, signal.BitIndex), 
                           new SignalData()
                           {
                               Position = loadPlace,
                               ExchangeDirection = ExchangeDirection.Load,
                               ExchangeType = exchageType,
                               Signal = signal
                           });
        }

        private void AddUnloadSignal(int loadPlace, ExchangeType exchageType, IBitData signal)
        {
            _signalMap.Add(new Tuple<int, int>(signal.Register, signal.BitIndex), 
                           new SignalData()
                           {
                               Position = loadPlace,
                               ExchangeDirection = ExchangeDirection.Unload,
                               ExchangeType = exchageType,
                               Signal = signal
                           });
        }

        #endregion

        #region state machine

        public override bool DataChange(int register, int bit, bool value)
        {
            bool result = base.DataChange(register, bit, value);

            if (!result)
            {
                if (value && _signalMap.TryGetValue(new Tuple<int, int>(register, bit), out SignalData sd))
                {
                    SetNextState(sd);

                    result = true;
                }
            }

            return result;
        }

        private void SetNextState(SignalData sd)
        {
            Reset();
            Context.State = GetNextState(sd.Position, sd.ExchangeDirection, sd.ExchangeType);
            Context.State.Start();
            Context = null;
        }

        private Interfaces.IState GetNextState(int position, ExchangeDirection exchangeDirection, ExchangeType exchangeType)
        {
            switch (exchangeDirection)
            {
                case ExchangeDirection.Load:
                    return GeNextLoadState(position, exchangeType);
                case ExchangeDirection.Unload:
                    return GetNextUnloadState(position, exchangeType);
                default:
                    throw new NotImplementedException();
            }
        }

        private Interfaces.IState GetNextUnloadState(int position, ExchangeType exchangeType)
        {
            switch (exchangeType)
            {
                case ExchangeType.OnClamp:
                    return new UnloadingOnClampState() { PanelExchangeZone = position, Context = Context };
                case ExchangeType.OnBelt:
                    return new UnloadingOnBeltState() { PanelExchangeZone = position, Context = Context };
                default:
                    throw new NotImplementedException();
            }
        }

        private Interfaces.IState GeNextLoadState(int position, ExchangeType exchangeType)
        {
            switch (exchangeType)
            {
                case ExchangeType.OnStop:
                    return new LoadingOnStopState() { PanelExchangeZone = position, Context = Context };
                case ExchangeType.OnBelt:
                    return new LoadingOnBeltState() { PanelExchangeZone = position, Context = Context };
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        private void CheckForActiveSignal()
        {
            SignalData activeSignal = null;

            foreach (var item in _signalMap.Values)
            {
                if(GetValue(item.Signal))
                {
                    //SetNextState(item);
                    activeSignal = item;
                    break;
                }
            }

            if (activeSignal != null) SetNextState(activeSignal);
        }
    }
}
