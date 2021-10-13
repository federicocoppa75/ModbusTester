using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderSimulator.StateMachine.Tests.Common
{
    public class DummyContext : IContext
    {
        public IState State { get ; set; }

        public virtual void LoadPanel(int loadPosition, ExchangeType exchangeType)
        {
            throw new NotImplementedException();
        }

        public virtual void PreLoadPanel(int loadPosition, ExchangeType exchangeType)
        {
            throw new NotImplementedException();
        }

        public void Log(LogType type, string message)
        {
            throw new NotImplementedException();
        }

        //public void OnStatusChanged()
        //{
        //    //throw new NotImplementedException();
        //}

        public virtual void RequestLoadPanelExcutionConferm(int loadPosition, ExchangeType exchangeType, Action confermAction)
        {
            throw new NotImplementedException();
        }

        public virtual void RequestMachineAbortAck(Action ackAction)
        {
            throw new NotImplementedException();
        }

        public virtual void UnloadPanel(int loadPosition, ExchangeType exchangeType)
        {
            throw new NotImplementedException();
        }

        public virtual void PreUnloadPanel(int loadPosition, ExchangeType exchangeType)
        {
            throw new NotImplementedException();
        }
    }
}
