using Registers.Models;
using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Utils.Factories
{
    public static class ModelFactory
    {
        public static BaseData Create(IBaseData obj)
        {
            if(obj is IClockData cd)
            {
                return new ClockData()
                {
                    BitIndex = cd.BitIndex,
                    DataDirection = cd.DataDirection,
                    DataCategory = cd.DataCategory,
                    Name = cd.Name,
                    Register = cd.Register,
                    Period = cd.Period
                };
            }
            else if(obj is IBitData bd)
            {
                return new BitData()
                {
                    BitIndex = bd.BitIndex,
                    DataDirection = bd.DataDirection,
                    DataCategory = bd.DataCategory,
                    Name = bd.Name,
                    Register = bd.Register
                };
            }
            else if(obj is IValueData vd)
            {
                return new ValueData()
                {
                    DataDirection = vd.DataDirection,
                    DataCategory = vd.DataCategory,
                    Name = vd.Name,
                    Register = vd.Register
                };
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
