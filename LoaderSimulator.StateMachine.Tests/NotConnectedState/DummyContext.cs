using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderSimulator.StateMachine.Tests.NotConnectedState
{
    class DummyContext : Common.DummyContext
    {
        public DummyContext()
        {
            Messenger.Default.Register<GetSignalMessage>(this, OnGetSignalMessage);
        }

        private void OnGetSignalMessage(GetSignalMessage msg)
        {
            msg.SetSignal(msg.Signal, new Common.DummySignal());
        }
    }
}
