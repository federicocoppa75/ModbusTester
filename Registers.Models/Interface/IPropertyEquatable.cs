using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Models.Interface
{
    public interface IPropertyEquatable<T>
    {
        bool PropEquals(T value);
    }
}
