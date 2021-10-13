using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Registers.ViewModels;
using Registers.ViewModels.Messages;

namespace LoaderSimulator.ViewModels
{
    public class MachineStatusViewModel : ViewModelBase
    {
        public ObservableCollection<BaseDataViewModel> DataItems { get; set; } = new ObservableCollection<BaseDataViewModel>();

        public MachineStatusViewModel() : base()
        {
            MessengerInstance.Register<LoadInputDataMessage>(this, OnLoadInputDataMessage);
        }

        private void OnLoadInputDataMessage(LoadInputDataMessage msg)
        {
            DataItems.Clear();

            msg.Items.Where((o) => o.DataCategory == Registers.Models.Enums.DataCategory.Status)
                     .ToList()
                     .ForEach((o) => DataItems.Add(o));
        }
    }
}
