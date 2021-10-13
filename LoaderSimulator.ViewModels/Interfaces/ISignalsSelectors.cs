using LoaderSimulator.StateMachine.Messages;
using Registers.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.ViewModels.Interfaces
{
    public interface ISignalsSelectors
    {
        void GetSignalForLoad(GetSignalForStartLoadMessage msg);
        void GetSignalForUnload(GetSignalForStartUnloadMessage msg);
        void GetSignalsForPieceExchange(GetSignalsForPieceExchangeMessage msg);
        void GetSignal(GetSignalMessage msg);

    }
}
