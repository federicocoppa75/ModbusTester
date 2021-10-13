


using System;
using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Messages;
using Registers.Models;

namespace LoaderSimulator.StateMachine.Tests.IdleState
{
    class DummyContext : Common.DummyContext
    {
        // manage ABORT
        public Common.DummySignal MachineAbortSignal { get; set; }
        public Common.DummySignal MachineAckSignal { get; set; }
        public Common.DummySignal LoaderAbortSignal { get; set; }
        public Common.DummySignal LoaderAckSignal { get; set; }




        public DummyContext() : base()
        {
            Messenger.Default.Register<GetSignalForStartLoadMessage>(this, OnGetSignalForStartLoadMessage);
            Messenger.Default.Register<GetSignalForStartUnloadMessage>(this, OnGetSignalForStartUnladMessage);
            Messenger.Default.Register<GetSignalMessage>(this, OnGetSignalMessage);
            Messenger.Default.Register<GetSignalsForPieceExchangeMessage>(this, OnGetSignalsForPieceExchangeMessage);
        }

        private void OnGetSignalMessage(GetSignalMessage msg)
        {

            switch (msg.Signal)
            {
                case Enums.Signals.SCM_ABORT_REQ:
                    msg.SetSignal(msg.Signal, MachineAbortSignal ?? (MachineAbortSignal = new Common.DummySignal() { Register = 1100, BitIndex = 0 }));
                    break;
                case Enums.Signals.SCM_ABORT_ACK:
                    msg.SetSignal(msg.Signal, MachineAckSignal ?? (MachineAckSignal = new Common.DummySignal() { Register = 1100, BitIndex = 1 }));
                    break;
                case Enums.Signals.EX_ABORT_REQ:
                    msg.SetSignal(msg.Signal, LoaderAbortSignal ?? (LoaderAbortSignal = new Common.DummySignal() { Register = 1100, BitIndex = 2 }));
                    break;
                case Enums.Signals.EX_ABORT_ACK:
                    msg.SetSignal(msg.Signal, LoaderAckSignal ?? (LoaderAckSignal = new Common.DummySignal() { Register = 1100, BitIndex = 3 }));
                    break;
                default:
                    msg.SetSignal(msg.Signal, new Common.DummySignal() { Register = 0, BitIndex = 0 });
                    break;
            }            
        }

        private void OnGetSignalForStartLoadMessage(GetSignalForStartLoadMessage msg)
        {
            msg.SetSignal(1, Enums.ExchangeType.OnStop, new Common.DummySignal() { Register = 1000, BitIndex = 0 });
            msg.SetSignal(1, Enums.ExchangeType.OnStop, new Common.DummySignal() { Register = 1000, BitIndex = 1 });
            msg.SetSignal(2, Enums.ExchangeType.OnStop, new Common.DummySignal() { Register = 1000, BitIndex = 2 });
            msg.SetSignal(2, Enums.ExchangeType.OnStop, new Common.DummySignal() { Register = 1000, BitIndex = 3 });
            msg.SetSignal(3, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Register = 1000, BitIndex = 4 });
            msg.SetSignal(3, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Register = 1000, BitIndex = 5 });
        }


        private void OnGetSignalForStartUnladMessage(GetSignalForStartUnloadMessage msg)
        {
            msg.SetSignal(2, Enums.ExchangeType.OnClamp, new Common.DummySignal() { Register = 1010, BitIndex = 2 });
            msg.SetSignal(2, Enums.ExchangeType.OnClamp, new Common.DummySignal() { Register = 1010, BitIndex = 3 });
            msg.SetSignal(3, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Register = 1010, BitIndex = 4 });
            msg.SetSignal(3, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Register = 1010, BitIndex = 5 });
            msg.SetSignal(4, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Register = 1010, BitIndex = 6 });
            msg.SetSignal(4, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Register = 1010, BitIndex = 7 });
        }

        private void OnGetSignalsForPieceExchangeMessage(GetSignalsForPieceExchangeMessage msg)
        {
            var i = 0;

            foreach (var item in msg.SignalsRequest)
            {
                msg.SetSignal(item, new Common.DummySignal() { Register = 1020, BitIndex = i++ });

            }
        }

        public override void RequestMachineAbortAck(Action ackAction) => ackAction();
    }
}
