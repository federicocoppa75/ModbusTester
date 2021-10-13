using Registers.Models.Enums;
using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Registers.Models
{
    [XmlInclude(typeof(BitData))]
    [XmlInclude(typeof(ValueData))]
    [XmlInclude(typeof(ClockData))]
    public class BaseData : IBaseData
    {
        public string Name { get; set; }
        public int Register { get; set; }
        public DataDirection DataDirection { get; set; }
        public DataCategory DataCategory { get; set; }
    }
}
