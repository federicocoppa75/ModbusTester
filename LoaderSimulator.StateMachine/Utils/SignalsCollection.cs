using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderSimulator.StateMachine.Utils
{
    public class SignalsCollection
    {
        private Dictionary<Enums.Signals, IBitData> _signalsById = new Dictionary<Enums.Signals, IBitData>();
        private Dictionary<Tuple<int, int>, IBitData> _signalByAddress = new Dictionary<Tuple<int, int>, IBitData>();
        private Dictionary<Tuple<int, int>, Enums.Signals> _signalIdByAddress = new Dictionary<Tuple<int, int>, Enums.Signals>();

        public SignalsCollection()
        {

        }

        public void Add(Enums.Signals signalId, IBitData signal)
        {
            var t = new Tuple<int, int>(signal.Register, signal.BitIndex);
            _signalsById.Add(signalId, signal);
            _signalByAddress.Add(t, signal);
            _signalIdByAddress.Add(t, signalId);
        }

        public bool TryGetValue(Enums.Signals signalId, out IBitData signal) => _signalsById.TryGetValue(signalId, out signal);

        public bool TryGetValue(int register, int bitIndex, out IBitData signal) => _signalByAddress.TryGetValue(new Tuple<int, int>(register, bitIndex), out signal);

        public bool TryGetValue(int register, int bitIndex, out Enums.Signals signalId) => _signalIdByAddress.TryGetValue(new Tuple<int, int>(register, bitIndex), out signalId);

        public IList<IBitData> ToList() => _signalsById.Values.ToList();

        public void Clear()
        {
            _signalsById.Clear();
            _signalByAddress.Clear();
            _signalIdByAddress.Clear();
        }
    }
}
