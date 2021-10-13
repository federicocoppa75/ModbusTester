using GalaSoft.MvvmLight;
using Registers.Models.Enums;
using Registers.ViewModels;
using Registers.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LoaderSimulator.ViewModels
{
    public class ParkRequestViewModel : ViewModelBase
    {
        public string Title => "Park (Machine -> loader)";

        public ObservableCollection<BaseDataViewModel> DataItems { get; set; } = new ObservableCollection<BaseDataViewModel>();


        public ParkRequestViewModel() : base()
        {
            MessengerInstance.Register<LoadAllDataMessage>(this, OnLoadAllDataMessage);

        }

        private void OnLoadAllDataMessage(LoadAllDataMessage msg)
        {
            DataItems.Clear();

            msg.Items.Where((o) => o.DataCategory == DataCategory.Park)
                     .ToList()
                     .ForEach((o) => DataItems.Add(o));
        }
    }
}
