using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.StateMachine.Messages;
using LoaderSimulator.ViewModels.Interfaces;
using Registers.Models.Interface;
using Registers.ViewModels;
using Registers.ViewModels.Enums;
using Registers.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderSimulator.ViewModels.Helpers
{
    public class SignalsSelector : ISignalsSelectors
    {
        const string _preloadStr = "SCM_PRELOAD_REQ_";
        const string _loadStr = "SCM_LOAD_REQ_";
        const string _preunloadStr = "SCM_PREUNLOAD_REQ_";
        const string _unloadStr = "SCM_UNLOAD_REQ_";

        List<BaseDataViewModel> _signals = new List<BaseDataViewModel>();

        public SignalsSelector()
        {
            Messenger.Default.Register<LoadAllDataMessage>(this, OnLoadAllDataMessage);
        }

        private void OnLoadAllDataMessage(LoadAllDataMessage msg)
        {
            _signals.Clear();

            msg.Items.ForEach((o) => _signals.Add(o));
        }

        public void GetSignal(GetSignalMessage msg)
        {
            switch (msg.Signal)
            {
                case Signals.SCM_ABORT_REQ:
                    msg.SetSignal(Signals.SCM_ABORT_REQ, GetSignal(_signals, "SCM_ABORT_REQ"));
                    break;
                case Signals.SCM_ABORT_ACK:
                    msg.SetSignal(Signals.SCM_ABORT_ACK, GetSignal(_signals, "SCM_ABORT_ACK"));
                    break;
                case Signals.EX_ABORT_REQ:
                    msg.SetSignal(Signals.EX_ABORT_REQ, GetSignal(_signals, "EX_ABORT_REQ"));
                    break;
                case Signals.EX_ABORT_ACK:
                    msg.SetSignal(Signals.EX_ABORT_ACK, GetSignal(_signals, "EX_ABORT_ACK"));
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public void GetSignalForLoad(GetSignalForStartLoadMessage msg)
        {
            foreach (var item in _signals)
            {
                if (item.DataType == DataType.Bit)
                {
                    if (item.Name.Contains(_preloadStr) || item.Name.Contains(_loadStr))
                    {
                        var pos = GetPositionFromName(item.Name);
                        var et = GetExchageTypeForLoad(pos);

                        msg.SetSignal?.Invoke(pos, et, item as IBitData);
                    }
                }
            }
        }

        public void GetSignalForUnload(GetSignalForStartUnloadMessage msg)
        {
            foreach (var item in _signals)
            {
                if (item.DataType == DataType.Bit)
                {
                    if (item.Name.Contains(_preunloadStr) || item.Name.Contains(_unloadStr))
                    {
                        var pos = GetPositionFromName(item.Name);
                        var et = GetExchageTypeForUnload(pos);

                        msg.SetSignal?.Invoke(pos, et, item as IBitData);
                    }
                }
            }
        }

        public void GetSignalsForPieceExchange(GetSignalsForPieceExchangeMessage msg)
        {
            switch (msg.ExchangeDirection)
            {
                case ExchangeDirection.Load:
                    GetSignalsForPieceLoad(_signals, msg);
                    break;
                case ExchangeDirection.Unload:
                    GetSignalsForPieceUnoad(_signals, msg);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }


        private int GetPositionFromName(string name)
        {
            int result = -1;
            
            if(name.Contains(_preloadStr))
            {
                var s = name.Replace(_preloadStr, "");

                if(int.TryParse(s, out int i))
                {
                    result = i;
                }
            }
            else if(name.Contains(_loadStr))
            {
                var s = name.Replace(_loadStr, "");

                if (int.TryParse(s, out int i))
                {
                    result = i;
                }
            }
            if (name.Contains(_preunloadStr))
            {
                var s = name.Replace(_preunloadStr, "");

                if (int.TryParse(s, out int i))
                {
                    result = i;
                }
            }
            else if (name.Contains(_unloadStr))
            {
                var s = name.Replace(_unloadStr, "");

                if (int.TryParse(s, out int i))
                {
                    result = i;
                }
            }

            return result;
        }

        private ExchangeType GetExchageTypeForLoad(int pos)
        {
            switch (pos)
            {
                case 1:
                case 2:
                    return ExchangeType.OnStop;
                case 3:
                    return ExchangeType.OnBelt;
                default:
                    throw new NotImplementedException();
            }
        }

        private ExchangeType GetExchageTypeForUnload(int pos)
        {
            switch (pos)
            {
                case 2:
                    return ExchangeType.OnClamp;
                case 3:
                case 4:
                    return ExchangeType.OnBelt;
                default:
                    throw new NotImplementedException();
            }
        }

        private void GetSignalsForPieceUnoad(IList<BaseDataViewModel> signals, GetSignalsForPieceExchangeMessage msg)
        {
            switch (msg.Position)
            {
                case 2:
                    GetSignalsForPieceUnoadOnClamp(signals, msg);
                    break;
                case 3:
                case 4:
                    GetSignalsForPieceUnoadOnBelt(signals, msg);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void GetSignalsForPieceLoad(IList<BaseDataViewModel> signals, GetSignalsForPieceExchangeMessage msg)
        {
            switch (msg.Position)
            {
                case 1:
                case 2:
                    GetSignalsForPieceLoadOnStop(signals, msg);
                    break;
                case 3:
                    GetSignalsForPieceLoadOnBelt(signals, msg);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void GetSignalsForPieceLoadOnStop(IList<BaseDataViewModel> signals, GetSignalsForPieceExchangeMessage msg)
        {
            if (msg.ExchangeType != ExchangeType.OnStop) throw new NotImplementedException();


            foreach (var item in msg.SignalsRequest)
            {
                switch (item)
                {
                    case Signals.SCM_PHOTOCELL:
                        switch (msg.Position)
                        {
                            case 1:
                                msg.SetSignal(Signals.SCM_PHOTOCELL, GetSignal(signals, "SCM_PHOTOCELL_1"));
                                break;
                            case 2:
                                msg.SetSignal(Signals.SCM_PHOTOCELL, GetSignal(signals, "SCM_PHOTOCELL_2"));
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    case Signals.EX_NO_INTERFERENCE:
                        msg.SetSignal(Signals.EX_NO_INTERFERENCE, GetSignal(signals, "EX_NO_INTERFERENCE1"));
                        break;
                    case Signals.SCM_LOAD_REQ:
                        switch (msg.Position)
                        {
                            case 1:
                                msg.SetSignal(Signals.SCM_LOAD_REQ, GetSignal(signals, "SCM_LOAD_REQ_1"));
                                break;
                            case 2:
                                msg.SetSignal(Signals.SCM_LOAD_REQ, GetSignal(signals, "SCM_LOAD_REQ_2"));
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    case Signals.SCM_PIECE_TAKEN:
                        switch (msg.Position)
                        {
                            case 1:
                                msg.SetSignal(Signals.SCM_PIECE_TAKEN, GetSignal(signals, "SCM_PIECE_TAKEN_1"));
                                break;
                            case 2:
                                msg.SetSignal(Signals.SCM_PIECE_TAKEN, GetSignal(signals, "SCM_PIECE_TAKEN_2"));
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    case Signals.EX_PIECE_READY:
                        switch (msg.Position)
                        {
                            case 1:
                                msg.SetSignal(Signals.EX_PIECE_READY, GetSignal(signals, "EX_PIECE_READY_1"));
                                break;
                            case 2:
                                msg.SetSignal(Signals.EX_PIECE_READY, GetSignal(signals, "EX_PIECE_READY_2"));
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    case Signals.EX_LOAD_END:
                        switch (msg.Position)
                        {
                            case 1:
                                msg.SetSignal(Signals.EX_LOAD_END, GetSignal(signals, "EX_LOAD_END_1"));
                                break;
                            case 2:
                                msg.SetSignal(Signals.EX_LOAD_END, GetSignal(signals, "EX_LOAD_END_2"));
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void GetSignalsForPieceLoadOnBelt(IList<BaseDataViewModel> signals, GetSignalsForPieceExchangeMessage msg)
        {
            if (msg.ExchangeType != ExchangeType.OnBelt) throw new NotImplementedException();
            if (msg.Position != 3) throw new NotImplementedException();

            foreach (var item in msg.SignalsRequest)
            {
                switch (item)
                {
                    case Signals.SCM_PHOTOCELL:
                        msg.SetSignal(Signals.SCM_PHOTOCELL, GetSignal(signals, "SCM_PHOTOCELL_3"));
                        break;
                    case Signals.EX_NO_INTERFERENCE:
                        msg.SetSignal(Signals.EX_NO_INTERFERENCE, GetSignal(signals, "EX_NO_INTERFERENCE1"));
                        break;
                    case Signals.SCM_LOAD_REQ:
                        msg.SetSignal(Signals.SCM_LOAD_REQ, GetSignal(signals, "SCM_LOAD_REQ_3"));
                        break;
                    case Signals.SCM_PIECE_TAKEN:
                        msg.SetSignal(Signals.SCM_PIECE_TAKEN, GetSignal(signals, "SCM_PIECE_TAKEN_3"));
                        break;
                    case Signals.EX_PIECE_READY:
                        msg.SetSignal(Signals.EX_PIECE_READY, GetSignal(signals, "EX_PIECE_READY_3"));
                        break;
                    case Signals.EX_LOAD_END:
                        msg.SetSignal(Signals.EX_LOAD_END, GetSignal(signals, "EX_LOAD_END_3"));
                        break;
                    default:
                        break;
                }
            }
        }

        private void GetSignalsForPieceUnoadOnClamp(IList<BaseDataViewModel> signals, GetSignalsForPieceExchangeMessage msg)
        {
            if (msg.ExchangeType != ExchangeType.OnClamp) throw new NotImplementedException();
            if (msg.Position != 2) throw new NotImplementedException();

            foreach (var item in msg.SignalsRequest)
            {
                switch (item)
                {
                    case Signals.SCM_PHOTOCELL:
                        msg.SetSignal(Signals.SCM_PHOTOCELL, GetSignal(signals, "SCM_PHOTOCELL_2"));
                        break;
                    case Signals.EX_NO_INTERFERENCE:
                        msg.SetSignal(Signals.EX_NO_INTERFERENCE, GetSignal(signals, "EX_NO_INTERFERENCE1"));
                        break;
                    case Signals.SCM_UNLOAD_REQ:
                        msg.SetSignal(Signals.SCM_UNLOAD_REQ, GetSignal(signals, "SCM_UNLOAD_REQ_2"));
                        break;
                    case Signals.EX_UNLOAD_END:
                        msg.SetSignal(Signals.EX_UNLOAD_END, GetSignal(signals, "EX_UNLOAD_END_2"));
                        break;
                    case Signals.SCM_PIECE_GIVEN:
                        msg.SetSignal(Signals.SCM_PIECE_GIVEN, GetSignal(signals, "SCM_PIECE_GIVEN_2"));
                        break;
                    case Signals.EX_PIECE_REQ:
                        msg.SetSignal(Signals.EX_PIECE_REQ, GetSignal(signals, "EX_PIECE_REQ_2"));
                        break;
                    default:
                        break;
                }
            }

        }

        private void GetSignalsForPieceUnoadOnBelt(IList<BaseDataViewModel> signals, GetSignalsForPieceExchangeMessage msg)
        {
            if (msg.ExchangeType != ExchangeType.OnBelt) throw new NotImplementedException();

            foreach (var item in msg.SignalsRequest)
            {
                switch (item)
                {
                    case Signals.SCM_PHOTOCELL:
                        switch (msg.Position)
                        {
                            case 3:
                                msg.SetSignal(Signals.SCM_PHOTOCELL, GetSignal(signals, "SCM_PHOTOCELL_3"));
                                break;
                            case 4:
                                msg.SetSignal(Signals.SCM_PHOTOCELL, GetSignal(signals, "SCM_PHOTOCELL_4"));
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    case Signals.EX_NO_INTERFERENCE:
                        switch (msg.Position)
                        {
                            case 3:
                                msg.SetSignal(Signals.EX_NO_INTERFERENCE, GetSignal(signals, "EX_NO_INTERFERENCE1"));
                                break;
                            case 4:
                                msg.SetSignal(Signals.EX_NO_INTERFERENCE, GetSignal(signals, "EX_NO_INTERFERENCE2"));
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    case Signals.SCM_UNLOAD_REQ:
                        switch (msg.Position)
                        {
                            case 3:
                                msg.SetSignal(Signals.SCM_UNLOAD_REQ, GetSignal(signals, "SCM_UNLOAD_REQ_3"));
                                break;
                            case 4:
                                msg.SetSignal(Signals.SCM_UNLOAD_REQ, GetSignal(signals, "SCM_UNLOAD_REQ_4"));
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    case Signals.EX_UNLOAD_END:
                        switch (msg.Position)
                        {
                            case 3:
                                msg.SetSignal(Signals.EX_UNLOAD_END, GetSignal(signals, "EX_UNLOAD_END_3"));
                                break;
                            case 4:
                                msg.SetSignal(Signals.EX_UNLOAD_END, GetSignal(signals, "EX_UNLOAD_END_4"));
                                break;
                            default:
                                throw new ArgumentException();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private IBitData GetSignal(IList<BaseDataViewModel> signals, string name)
        {
            var s = signals.First((o) => string.Compare(o.Name, name) == 0);

            return s as IBitData;
        }

        //private string GetNameOfSignalForLoadOnStop(GetSignalsForPieceExchangeMessage msg)
        //{
        //    return "";
        //}

        //private string GetNameOfSignalForLoadOnBelt(GetSignalsForPieceExchangeMessage msg)
        //{
        //    return "";
        //}

        //private string GetNameOfSignalForUnloadOnBelt(GetSignalsForPieceExchangeMessage msg)
        //{
        //    return "";
        //}

        //private string GetNameOfSignalForLoadOnClamp(GetSignalsForPieceExchangeMessage msg)
        //{
        //    return "";
        //}
    }
}
