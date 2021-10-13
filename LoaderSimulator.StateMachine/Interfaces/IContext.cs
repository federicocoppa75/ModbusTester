using LoaderSimulator.StateMachine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine.Interfaces
{
    public interface IContext
    {
        IState State { get; set; }

        void LoadPanel(int loadPosition, ExchangeType exchangeType);
        void PreLoadPanel(int loadPosition, ExchangeType exchangeType);

        void UnloadPanel(int loadPosition, ExchangeType exchangeType);
        void PreUnloadPanel(int loadPosition, ExchangeType exchangeType);

        void RequestLoadPanelExcutionConferm(int loadPosition, ExchangeType exchangeType, Action confermAction);

        void RequestMachineAbortAck(Action ackAction);

        //void OnStatusChanged();

        void Log(LogType type, string message);
    }
}
