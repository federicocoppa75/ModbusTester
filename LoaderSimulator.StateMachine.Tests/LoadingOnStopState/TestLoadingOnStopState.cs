using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderSimulator.StateMachine.Tests.LoadingOnStopState
{
    [TestClass]
    public class TestLoadingOnStopState
    {
        [TestMethod]
        public void ExecuteLoadingTest()
        {
            DummyContext context = new DummyContext();
            var state = new StateMachine.LoadingOnStopState() { Context = context };

            state.Start();

            context.ScmLoadRequest.Value = true;

            Assert.IsFalse(context.ExNoInterference.Value);

            //context.ScmPhotocell.Value = true;

            Assert.IsTrue(context.ExPieceReady.Value);

            context.ScmLoadRequest.Value = false;
            context.ScmPieceTaken.Value = true;

            Assert.IsTrue(context.ExNoInterference.Value);
            Assert.IsFalse(context.ExPieceReady.Value);
            Assert.IsTrue(context.ExLoadEnd.Value);

            context.ScmPieceTaken.Value = false;

            Assert.IsFalse(context.ExLoadEnd.Value);
            Assert.IsInstanceOfType(context.State, typeof(StateMachine.IdleState));
        }

        [TestMethod]
        public void ExecuteLoadingWithAbortByMachineTest()
        {
            DummyContext context = new DummyContext();
            var state = new StateMachine.LoadingOnStopState() { Context = context };

            state.Start();

            context.ScmLoadRequest.Value = true;

            Assert.IsFalse(context.ExNoInterference.Value);

            Assert.IsTrue(context.ExPieceReady.Value);

            context.ScmAbortSignal.Value = true;

            Assert.IsTrue(context.ExAckSignal.Value);

            Assert.IsInstanceOfType(context.State, typeof(WaitingForMachineAbortAckState));

            context.ScmAbortSignal.Value = false;

            Assert.IsFalse(context.ExAckSignal.Value);

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.IdleState));
        }

        [TestMethod]
        public void ExecuteLoadingWithAbortByLoaderTest()
        {
            DummyContext context = new DummyContext();
            var state = new StateMachine.LoadingOnStopState() { Context = context };

            state.Start();

            context.ScmLoadRequest.Value = true;

            Assert.IsFalse(context.ExNoInterference.Value);

            Assert.IsTrue(context.ExPieceReady.Value);

            context.ExAbortSignal.Value = true;

            Assert.IsInstanceOfType(context.State, typeof(WaitingForLoaderAbortAckState));

            context.ScmAckSignal.Value = true;

            Assert.IsFalse(context.ExAbortSignal.Value);

            context.ExAbortSignal.Value = false;

            Assert.IsInstanceOfType(context.State, typeof(StateMachine.IdleState));
        }
    }
}
