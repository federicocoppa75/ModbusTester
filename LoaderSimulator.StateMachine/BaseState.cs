using GalaSoft.MvvmLight.Messaging;
using LoaderSimulator.StateMachine.Interfaces;
using Registers.Models.Interface;
using Registers.ViewModels.Interfaces;
using Registers.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoaderSimulator.StateMachine
{
    public abstract class BaseState : IState
    {
        public abstract string Name { get; }
        public bool IsActive { get; protected set; }
        public IContext Context { get; set; }

        public abstract void Start();
        public abstract void Reset();

        protected bool IsSameSignal(IBitData signal, int register, int bit) => (signal.Register == register) && (signal.BitIndex == bit);

        protected bool GetValue(IBitData signal)
        {
            if (signal is IValueProvider<bool> v)
            {
                return v.Value;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected void SetValue(IBitData signal, bool value)
        {
            if (signal is IValueSetter<bool> v)
            {
                v.Value = value;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected void PulseValue(IBitData signal)
        {
            if (signal is IValueSetter<bool> v)
            {
                v.Value = true;

                Task.Delay(300)
                    .ContinueWith((t) => v.Value = false);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected void RegisterSignalObserver(IBitData signal, IBitObserver observer)
        {
            Messenger.Default.Send(new RegisterBitObserverMessage()
            {
                Register = signal.Register,
                BitIndex = signal.BitIndex,
                Observer = observer
            });
        }
    }
}
