using Registers.Comunication.Messages;
using Registers.Models.Interface;
using Registers.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Registers.ViewModels
{
    public class ValueDataViewModel : BaseDataViewModel<int>, IValueData, IBaseData
    {
        [Browsable(false)]
        public string BitIndex => "--";

        public override DataType DataType => DataType.Int;

        public ValueDataViewModel() : base()
        {

        }

        protected override void ApplyValueChanged(int value) => Value = value;

        protected override void NotifyValueChanged() => MessengerInstance.Send(new WriteValueMessage() { Register = Register, Value = Value });
    }
}
