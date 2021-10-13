using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Comunication.Messages
{
    public class WriteValueMessage
    {
        public int Register { get; set; }
        public int Value { get; set; }
    }
}
