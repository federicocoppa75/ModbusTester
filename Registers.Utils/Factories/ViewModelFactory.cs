using GalaSoft.MvvmLight.Ioc;
using Registers.Models.Interface;
using Registers.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Utils.Factories
{
    public static class ViewModelFactory
    {


        public static IBaseData Create(IBaseData obj)
        {
            if(obj is IClockData cd)
            {
                var factory = SimpleIoc.Default.GetInstance<IClockDataViewModelFactory>();
                var o = factory.Create();

                o.BitIndex = cd.BitIndex;
                o.DataDirection = cd.DataDirection;
                o.DataCategory = cd.DataCategory;
                o.Name = cd.Name;
                o.Register = cd.Register;
                o.Period = cd.Period;

                return o;
            }
            else if (obj is IBitData bd)
            {
                var factory = SimpleIoc.Default.GetInstance<IBitDataViewModelFactory>();
                var o = factory.Create();

                o.BitIndex = bd.BitIndex;
                o.DataDirection = bd.DataDirection;
                o.DataCategory = bd.DataCategory;
                o.Name = bd.Name;
                o.Register = bd.Register;

                return o;                
            }
            else if (obj is IValueData vd)
            {
                var factory = SimpleIoc.Default.GetInstance<IValueDataViewModelFactory>();
                var o = factory.Create();

                o.DataDirection = vd.DataDirection;
                o.DataCategory = vd.DataCategory;
                o.Name = vd.Name;
                o.Register = vd.Register;

                return o;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
