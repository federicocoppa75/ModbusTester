using Registers.Comunication.Messages;
using Registers.Models.Interface;
using Registers.ViewModels.Enums;
using Registers.ViewModels.Interfaces;
using Registers.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.ViewModels
{
    public class BitDataViewModel : BaseDataViewModel<bool>, IBitData, IBaseData
    {
        IBitObserver _observer;

        private int _bitIndex;

        public int BitIndex
        {
            get => _bitIndex;
            set => Set(ref _bitIndex, value, nameof(BitIndex));
        }

        public override DataType DataType => DataType.Bit;

        public BitDataViewModel() : base()
        {
            MessengerInstance.Register<RegisterBitObserverMessage>(this, OnRegisterBitObserverMessage);
            MessengerInstance.Register<UnregisterAllBitObserverMessage>(this, OnUnregisterAllBitObserverMessage);
        }

        protected override void ApplyValueChanged(int value)
        {
            int mask = 1 << BitIndex;
            int v = value & mask;
            bool b = v != 0;

            if (Value != b)
            {
                Value = b;

                if (_observer != null) _observer.DataChange(Register, BitIndex, b);
            }
        }

        protected override void NotifyValueChanged() => MessengerInstance.Send(new WriteBitValueMessage() { Register = Register, BitIndex = BitIndex, Value = Value });


        private void OnUnregisterAllBitObserverMessage(UnregisterAllBitObserverMessage msg) => _observer = null;

        private void OnRegisterBitObserverMessage(RegisterBitObserverMessage msg)
        {
            if((Register == msg.Register) && (BitIndex == msg.BitIndex))
            {
                _observer = msg.Observer;
            }
        }

    }
}
