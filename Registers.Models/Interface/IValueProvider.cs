using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Models.Interface
{
    public interface IValueProvider<T>
    {
        T Value { get; }
    }
}
