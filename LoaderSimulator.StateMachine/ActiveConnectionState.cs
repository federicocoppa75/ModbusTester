using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Interfaces;
using LoaderSimulator.StateMachine.Messages;
using Registers.Comunication.Messages.ComState;
using Registers.Models.Interface;
using Registers.ViewModels.Interfaces;
using Registers.ViewModels.Messages;

namespace LoaderSimulator.StateMachine
{
    public abstract class ActiveConnectionState : BaseState, IBitObserver, IAbortable
    {
        //protected Dictionary<Enums.Signals, IBitData> Signals { get; private set; } = new Dictionary<Enums.Signals, IBitData>();
        //protected Dictionary<Tuple<int, int>, IBitData> 
        //public SignalsCollection Signals { get; set; } = new SignalsCollection();
        IBitData _scmAbortRequestSignal;
        IBitData _exAbortRequestSignal;

        public ActiveConnectionState() : base()
        {
            Messenger.Default.Register<ConnectionToServerChangedMessage>(this, OnConnectionToServerChangedMessage);
        }

        private void OnConnectionToServerChangedMessage(ConnectionToServerChangedMessage msg)
        {
            if (IsActive && !msg.IsActive)
            {
                Reset();
                Context.State = new NotConnectedState() { Context = Context };
                Context.State.Start();
                Context = null;
            }
        }

        public override void Start()
        {
            RequestSignalToListen();

            IsActive = true;

            RegisterSignalObserver();
        }

        public override void Reset()
        {
            IsActive = false;
            Messenger.Default.Send(new UnregisterAllBitObserverMessage());
        }

        public virtual bool DataChange(int register, int bit, bool value)
        {
            bool result = false;

            if (IsActive && value)
            {
                if(IsSameSignal(_scmAbortRequestSignal, register, bit))
                {
                    GoToMachineAbortAck();
                    result = true;
                }
                else if(IsSameSignal(_exAbortRequestSignal, register, bit))
                {
                    GoToLoaderAbortAck();
                    result = true;
                }
            }

            return result;
        }

        protected virtual void RequestSignalToListen()
        {
            Messenger.Default.Send(new GetSignalMessage() { Signal = Enums.Signals.SCM_ABORT_REQ, SetSignal = (e, s) => _scmAbortRequestSignal = s });
            Messenger.Default.Send(new GetSignalMessage() { Signal = Enums.Signals.EX_ABORT_REQ, SetSignal = (e, s) => _exAbortRequestSignal = s });
        }

        protected virtual void RegisterSignalObserver()
        {
            RegisterSignalObserver(_scmAbortRequestSignal);
            RegisterSignalObserver(_exAbortRequestSignal);
        }

        protected void RegisterSignalObserver(IBitData signal) => RegisterSignalObserver(signal, this);


        //protected virtual void RegisterSignalObserver()
        //{
        //    foreach (var item in Signals.ToList())
        //    {
        //        Messenger.Default.Send(new RegisterBitObserverMessage()
        //        {
        //            Register = item.Register,
        //            BitIndex = item.BitIndex,
        //            Observer = this
        //        });
        //    }
        //}

        //protected virtual bool IsManaged(Enums.Signals signalId)
        //{
        //    bool result = false;

        //    switch (signalId)
        //    {
        //        case Enums.Signals.SCM_ABORT_REQ:
        //        case Enums.Signals.EX_ABORT_REQ:
        //            result = true;
        //            break;

        //        default:
        //            break;
        //    }

        //    return result;
        //}

        //protected virtual void Manage(Enums.Signals signalId, int register, int bit, bool value)
        //{
        //    switch (signalId)
        //    {
        //        case Enums.Signals.SCM_ABORT_REQ:
        //            break;
        //        case Enums.Signals.EX_ABORT_REQ:

        //        default:
        //            break;
        //    }
        //}

        private void GoToMachineAbortAck()
        {
            Reset();
            Context.State = new WaitingForMachineAbortAckState() { Context = Context };
            Context.State.Start();
            Context = null;
        }

        private void GoToLoaderAbortAck()
        {
            Reset();
            Context.State = new WaitingForLoaderAbortAckState() { Context = Context };
            Context.State.Start();
            Context = null;
        }

        public void Abort()
        {
            SetValue(_exAbortRequestSignal, true);
            GoToLoaderAbortAck();
        }
    }
}
