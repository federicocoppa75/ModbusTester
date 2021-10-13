using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine.Interfaces
{
    public interface IState
    {
        string Name { get; }
        void Start();
        void Reset();
    }
}
