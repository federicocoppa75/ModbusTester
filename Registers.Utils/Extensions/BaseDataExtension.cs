using Registers.Models;
using Registers.Models.Interface;
using Registers.Utils.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.Utils.Extensions
{
    public static class BaseDataExtension
    {
        public static BaseData ToModel(this IBaseData obj) => ModelFactory.Create(obj);
        
        public static T ToViewModel<T>(this IBaseData obj) where T : class, IBaseData
        {
            var ibd = ViewModelFactory.Create(obj);

            if(ibd is T vm)
            {
                return vm;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
