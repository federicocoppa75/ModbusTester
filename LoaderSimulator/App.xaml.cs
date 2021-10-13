using GalaSoft.MvvmLight.Ioc;
using LoaderSimulator.ViewModels.Helpers;
using LoaderSimulator.ViewModels.Interfaces;
using Registers.Models.Interface.UI;
using Registers.Utils.Interfaces;
using Registers.ViewModels.Utils.Factories;
using Registers.ViewModels.Utils.UI;
using System.Windows;

namespace LoaderSimulator
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherHelperEx.Initialize();

            SimpleIoc.Default.Register<IBitDataViewModelFactory, BitDataViewModelFactory>();
            SimpleIoc.Default.Register<IValueDataViewModelFactory, ValueDataViewModelFactory>();
            SimpleIoc.Default.Register<IClockDataViewModelFactory, ClockDataViewModelFactory>();
            SimpleIoc.Default.Register<ISignalsSelectors, SignalsSelector>();
            SimpleIoc.Default.Register<IDispatcherHelper, DispatcherHelperEx>();
        }
    }
}
