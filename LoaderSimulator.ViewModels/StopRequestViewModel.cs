using GalaSoft.MvvmLight;
using Registers.Models.Enums;
using Registers.ViewModels;
using Registers.ViewModels.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LoaderSimulator.ViewModels
{
    public class StopRequestViewModel : ViewModelBase
    {
        public string Title => "Stop (Loader -> machien)";

        public ObservableCollection<BaseDataViewModel> DataItems { get; set; } = new ObservableCollection<BaseDataViewModel>();


        public StopRequestViewModel() : base()
        {
            MessengerInstance.Register<LoadAllDataMessage>(this, OnLoadAllDataMessage);
        }

        private void OnLoadAllDataMessage(LoadAllDataMessage msg)
        {
            DataItems.Clear();

            msg.Items.Where((o) => o.DataCategory == DataCategory.Stop)
                     .ToList()
                     .ForEach((o) => DataItems.Add(o));
        }
    }
}
