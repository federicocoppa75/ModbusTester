using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.StateMachine.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderSimulator.StateMachine.Tests.LoadingOnStopState
{
    class DummyContext : Common.DummyContext
    {
        // manage abort   
        public Common.DummySignal ScmAbortSignal { get; set; }
        public Common.DummySignal ScmAckSignal { get; set; }
        public Common.DummySignal ExAbortSignal { get; set; }
        public Common.DummySignal ExAckSignal { get; set; }

        // manage loading
        public Common.DummySignal ScmPhotocell { get; set; }
        public Common.DummySignal ExNoInterference { get; set; }
        public Common.DummySignal ScmLoadRequest { get; set; }
        public Common.DummySignal ExPieceReady { get; set; }
        public Common.DummySignal ScmPieceTaken { get; set; }
        public Common.DummySignal ExLoadEnd { get; set; }


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
                    msg.SetSignal(msg.Signal, ScmAbortSignal ?? (ScmAbortSignal = new Common.DummySignal() { Name = "SCM_ABORT_REQ", Register = 1100, BitIndex = 0 }));
                    break;
                case Enums.Signals.SCM_ABORT_ACK:
                    msg.SetSignal(msg.Signal, ScmAckSignal ?? (ScmAckSignal = new Common.DummySignal() { Name = "SCM_ABORT_ACK", Register = 1100, BitIndex = 1 }));
                    break;
                case Enums.Signals.EX_ABORT_REQ:
                    msg.SetSignal(msg.Signal, ExAbortSignal ?? (ExAbortSignal = new Common.DummySignal() { Name = "EX_ABORT_REQ", Register = 1100, BitIndex = 2 }));
                    break;
                case Enums.Signals.EX_ABORT_ACK:
                    msg.SetSignal(msg.Signal, ExAckSignal ?? (ExAckSignal = new Common.DummySignal() { Name = "EX_ABORT_ACK", Register = 1100, BitIndex = 3 }));
                    break;
                default:
                    msg.SetSignal(msg.Signal, new Common.DummySignal() { Register = 0, BitIndex = 0 });
                    break;
            }
        }

        private void OnGetSignalForStartLoadMessage(GetSignalForStartLoadMessage msg)
        {
            msg.SetSignal(1, Enums.ExchangeType.OnStop, new Common.DummySignal() { Name = "", Register = 1000, BitIndex = 0 });
            msg.SetSignal(1, Enums.ExchangeType.OnStop, new Common.DummySignal() { Name = "", Register = 1000, BitIndex = 1 });
            msg.SetSignal(2, Enums.ExchangeType.OnStop, new Common.DummySignal() { Name = "", Register = 1000, BitIndex = 2 });
            msg.SetSignal(2, Enums.ExchangeType.OnStop, new Common.DummySignal() { Name = "", Register = 1000, BitIndex = 3 });
            msg.SetSignal(3, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Name = "", Register = 1000, BitIndex = 4 });
            msg.SetSignal(3, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Name = "", Register = 1000, BitIndex = 5 });
        }


        private void OnGetSignalForStartUnladMessage(GetSignalForStartUnloadMessage msg)
        {
            msg.SetSignal(2, Enums.ExchangeType.OnClamp, new Common.DummySignal() { Name = "", Register = 1010, BitIndex = 2 });
            msg.SetSignal(2, Enums.ExchangeType.OnClamp, new Common.DummySignal() { Name = "", Register = 1010, BitIndex = 3 });
            msg.SetSignal(3, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Name = "", Register = 1010, BitIndex = 4 });
            msg.SetSignal(3, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Name = "", Register = 1010, BitIndex = 5 });
            msg.SetSignal(4, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Name = "", Register = 1010, BitIndex = 6 });
            msg.SetSignal(4, Enums.ExchangeType.OnBelt, new Common.DummySignal() { Name = "", Register = 1010, BitIndex = 7 });
        }

        private void OnGetSignalsForPieceExchangeMessage(GetSignalsForPieceExchangeMessage msg)
        {
            var i = 0;

            foreach (var item in msg.SignalsRequest)
            {
                //msg.SetSignal(item, new Common.DummySignal() { Register = 1020, BitIndex = i++ });

                switch (item)
                {
                    //case Enums.Signals.SCM_ABORT_REQ:
                    //    break;
                    //case Enums.Signals.SCM_ABORT_ACK:
                    //    break;
                    //case Enums.Signals.EX_ABORT_REQ:
                    //    break;
                    //case Enums.Signals.EX_ABORT_ACK:
                    //    break;
                    case Enums.Signals.SCM_PHOTOCELL:
                        msg.SetSignal(item, ScmPhotocell ?? (ScmPhotocell = new Common.DummySignal() { Name = "SCM_PHOTOCELL", Register = 1030, BitIndex = 0 }));
                        break;
                    case Enums.Signals.EX_NO_INTERFERENCE:
                        msg.SetSignal(item, ExNoInterference ?? (ExNoInterference = new Common.DummySignal() { Name = "EX_NO_INTERFERENCE", Register = 1030, BitIndex = 1, Value = true }));
                        break;
                    //case Enums.Signals.SCM_PRELOAD_REQ:
                    //    break;
                    case Enums.Signals.SCM_LOAD_REQ:
                        msg.SetSignal(item, ScmLoadRequest ?? (ScmLoadRequest = new Common.DummySignal() { Name = "SCM_LOAD_REQ", Register = 1030, BitIndex = 2 }));
                        break;
                    case Enums.Signals.SCM_PIECE_TAKEN:
                        msg.SetSignal(item, ScmPieceTaken ?? (ScmPieceTaken = new Common.DummySignal() { Name = "SCM_PIECE_TAKEN", Register = 1030, BitIndex = 3 }));
                        break;
                    case Enums.Signals.EX_PIECE_READY:
                        msg.SetSignal(item, ExPieceReady ?? (ExPieceReady = new Common.DummySignal() { Name = "EX_PIECE_READY", Register = 1030, BitIndex = 4 }));
                        break;
                    case Enums.Signals.EX_LOAD_END:
                        msg.SetSignal(item, ExLoadEnd ?? (ExLoadEnd = new Common.DummySignal() { Name = "EX_LOAD_END", Register = 1030, BitIndex = 5 }));
                        break;
                    //case Enums.Signals.SCM_PREUNLOAD_REQ:
                    //    break;
                    //case Enums.Signals.SCM_UNLOAD_REQ:
                    //    break;
                    //case Enums.Signals.EX_UNLOAD_END:
                    //    break;
                    //case Enums.Signals.SCM_PIECE_GIVEN:
                    //    break;
                    //case Enums.Signals.EX_PIECE_REQ:
                    //    break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        
        public override void RequestMachineAbortAck(Action ackAction) => ackAction();

        public override void LoadPanel(int loadPosition, ExchangeType exchangeType) => ScmPhotocell.Value = true;

        public override void RequestLoadPanelExcutionConferm(int loadPosition, ExchangeType exchangeType, Action confermAction) => confermAction();
    }
}
