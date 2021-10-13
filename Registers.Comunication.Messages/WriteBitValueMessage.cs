using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Comunication.Messages
{
    public class WriteBitValueMessage
    {
        public int Register { get; set; }
        public int BitIndex { get; set; }
        public bool Value { get; set; }
    }
}
