using GalaSoft.MvvmLight.Ioc;
using Registers.Utils.Interfaces;
using Registers.ViewModels.Utils.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Server
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SimpleIoc.Default.Register<IBitDataViewModelFactory, BitDataViewModelFactory>();
            SimpleIoc.Default.Register<IValueDataViewModelFactory, ValueDataViewModelFactory>();
            SimpleIoc.Default.Register<IClockDataViewModelFactory, ClockDataViewModelFactory>();
        }
    }
}
