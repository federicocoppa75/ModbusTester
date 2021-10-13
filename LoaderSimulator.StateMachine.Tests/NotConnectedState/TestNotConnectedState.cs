using LoaderSimulator.StateMachine.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoaderSimulator.StateMachine;
using LoaderSimulator.StateMachine.Tests.Common;
using GalaSoft.MvvmLight.Messaging;
using Registers.Comunication.Messages.ComState;

namespace LoaderSimulator.StateMachine.Tests.NotConnectedState
{
    [TestClass]
    public class TestNotConnectedState
    {

        [TestMethod]
        public void TestChangeState()
        {
            IContext context = new DummyContext();
            context.State = new LoaderSimulator.StateMachine.NotConnectedState() { Context = context };
            context.State.Start();

            Messenger.Default.Send(new ConnectionToServerChangedMessage() { IsActive = true });

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.IdleState));
        }
    }
}
