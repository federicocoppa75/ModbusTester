using GalaSoft.MvvmLight.Messaging;
using Registers.Models.Enums;
using Registers.Models.Interface;
using Registers.ViewModels.Interfaces;
using Registers.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoaderSimulator.StateMachine.Tests.Common
{
    class DummySignal : IBitData, IValueSetter<bool>, IValueProvider<bool>
    {
        IBitObserver _observer;

        public DataCategory DataCategory { get; set; }
        public DataDirection DataDirection { get; set; }
        public string Name { get; set; }
        public int Register { get; set; }
        public int BitIndex { get; set; }

        private bool _value;
        public bool Value
        {
            get => _value;
            set
            {
                if(_value != value)
                {
                    _value = value;
                    Debug.WriteLine($"Register {Register}\tBit{BitIndex}\tValue {_value}\tName {Name}");
                    _observer?.DataChange(Register, BitIndex, _value);                    
                }
            }
        }


        public DummySignal()
        {
            Messenger.Default.Register<RegisterBitObserverMessage>(this, OnRegisterBitObserverMessage);
            Messenger.Default.Register<UnregisterAllBitObserverMessage>(this, OnUnregisterAllBitObserverMessage);
        }

        private void OnUnregisterAllBitObserverMessage(UnregisterAllBitObserverMessage obj)
        {
            _observer = null;
        }

        private void OnRegisterBitObserverMessage(RegisterBitObserverMessage msg)
        {
            if ((Register == msg.Register) && (BitIndex == msg.BitIndex))
            {
                _observer = msg.Observer;
            }
        }
    }
}
