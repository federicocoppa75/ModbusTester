using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Interfaces;
using LoaderSimulator.StateMachine.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Registers.Comunication.Messages.ComState;

namespace LoaderSimulator.StateMachine.Tests.IdleState
{
    [TestClass]
    public class TestIdleState
    {
        [TestMethod]
        public void TestChangeStateToNotConnected()
        {
            IContext context = new DummyContext();
            context.State = new StateMachine.IdleState() { Context = context };
            context.State.Start();

            Messenger.Default.Send(new ConnectionToServerChangedMessage() { IsActive = false });

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.NotConnectedState));
        }

        [TestMethod]
        public void TestChangeStateToLoadingOnStop1()
        {
            DummySignal ds = new DummySignal() { Register = 1000, BitIndex = 0 };
            IContext context = new DummyContext();
            context.State = new StateMachine.IdleState() { Context = context };
            context.State.Start();

            ds.Value = true;

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.LoadingOnStopState));
        }

        [TestMethod]
        public void TestChangeStateToLoadingOnStop2()
        {
            DummySignal ds = new DummySignal() { Register = 1000, BitIndex = 4 };
            IContext context = new DummyContext();
            context.State = new StateMachine.IdleState() { Context = context };
            context.State.Start();

            ds.Value = true;

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.LoadingOnBeltState));
        }

        [TestMethod]
        public void TestChangeStateToUnloadingOnStop2()
        {
            DummySignal ds = new DummySignal() { Register = 1010, BitIndex = 3 };
            IContext context = new DummyContext();
            context.State = new StateMachine.IdleState() { Context = context };
            context.State.Start();

            ds.Value = true;

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.UnloadingOnClampState));
        }

        [TestMethod]
        public void TestChangeStateToUnloadingOnOnBelt3()
        {
            DummySignal ds = new DummySignal() { Register = 1010, BitIndex = 5 };
            IContext context = new DummyContext();
            context.State = new StateMachine.IdleState() { Context = context };
            context.State.Start();

            ds.Value = true;

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.UnloadingOnBeltState));
        }

        [TestMethod]
        public void TestMachineAbort()
        {
            DummyContext context = new DummyContext();
            context.State = new StateMachine.IdleState() { Context = context };
            context.State.Start();

            context.MachineAbortSignal.Value = true;

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.WaitingForMachineAbortAckState));

            context.LoaderAckSignal.Value = true;
            context.MachineAbortSignal.Value = false;

            Assert.IsFalse(context.LoaderAckSignal.Value);
            Assert.IsInstanceOfType(context.State, typeof(StateMachine.IdleState));
        }

        [TestMethod]
        public void TestLoaderAbort()
        {
            DummyContext context = new DummyContext();
            context.State = new StateMachine.IdleState() { Context = context };
            context.State.Start();

            context.LoaderAbortSignal.Value = true;

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.WaitingForLoaderAbortAckState));

            context.MachineAckSignal.Value = true;

            Assert.IsFalse(context.LoaderAbortSignal.Value);
            Assert.IsInstanceOfType(context.State, typeof(StateMachine.IdleState));
        }
    }
}
