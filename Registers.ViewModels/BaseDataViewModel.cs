using GalaSoft.MvvmLight;
using Registers.Comunication.Messages;
using Registers.Models.Enums;
using Registers.Models.Interface;
using Registers.ViewModels.Enums;
using Registers.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.ViewModels
{
    public abstract class BaseDataViewModel : ViewModelBase, IBaseData, IPropertyEquatable<DataDirection>, IPropertyEquatable<DataType>
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value, nameof(Name));
        }

        private int _register;

        public int Register
        {
            get => _register;
            set => Set(ref _register, value, nameof(Register));
        }

        private DataDirection _dataDirection;

        public DataDirection DataDirection
        {
            get => _dataDirection;
            set => Set(ref _dataDirection, value, nameof(DataDirection));
        }

        private DataCategory _dataCategory;

        public DataCategory DataCategory
        {
            get => _dataCategory;
            set => Set(ref _dataCategory, value, nameof(DataCategory));
        }

        public abstract DataType DataType { get; }

        public BaseDataViewModel() : base()
        {
            MessengerInstance.Register<GetObservedVariableMessage>(this, OnGetObservedVariableMessage);
            MessengerInstance.Register<ValueChangedMessage>(this, OnValueChangedMessage);
        }


        private void OnGetObservedVariableMessage(GetObservedVariableMessage msg) => msg.Declare(Register, DataType);

        private void OnValueChangedMessage(ValueChangedMessage msg)
        {
            if(msg.Register == Register)
            {
                ApplyValueChanged(msg.Value);
            }
        }

        protected abstract void ApplyValueChanged(int value);

        public bool PropEquals(DataDirection value) => DataDirection == value;

        public bool PropEquals(DataType value) => DataType == value;
    }

    public abstract class BaseDataViewModel<T> : BaseDataViewModel, IValueSetter<T>, IValueProvider<T> where T : struct
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if(Set(ref _value, value, nameof(Value)))
                {
                    NotifyValueChanged();
                }
            }
        }


        public BaseDataViewModel() : base()
        {

        }

        protected abstract void NotifyValueChanged();

    }
}
