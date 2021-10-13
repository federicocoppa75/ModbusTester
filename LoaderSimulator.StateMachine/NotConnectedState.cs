using GalaSoft.MvvmLight.Messaging;
using Registers.Comunication.Messages.ComState;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine
{
    public class NotConnectedState : BaseState
    {
        public override string Name => "Not connected state";

        public NotConnectedState() : base()
        {
            Messenger.Default.Register<ConnectionToServerChangedMessage>(this, OnConnectionToServerChangedMessage);
        }

        private void OnConnectionToServerChangedMessage(ConnectionToServerChangedMessage msg)
        {
            if(IsActive && msg.IsActive)
            {
                Reset();
                Context.State = new IdleState() { Context = Context };
                Context.State.Start();
                Context = null;
            }
        }

        public override void Start()
        {
            IsActive = true;
        }

        public override void Reset()
        {
            IsActive = false;
        }
    }
}
