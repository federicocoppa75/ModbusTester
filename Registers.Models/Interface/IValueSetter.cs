using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Models.Interface
{
    public interface IValueSetter<T> : IValueProvider<T>
    {
        T Value { get; set; }
    }
}
