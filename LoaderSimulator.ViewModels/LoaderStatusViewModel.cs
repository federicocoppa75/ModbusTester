using GalaSoft.MvvmLight;
using Registers.ViewModels;
using Registers.ViewModels.Messages;
using System.Collections.ObjectModel;
using System.Linq;

namespace LoaderSimulator.ViewModels
{
    public class LoaderStatusViewModel : ViewModelBase
    {
        public ObservableCollection<BaseDataViewModel> DataItems { get; set; } = new ObservableCollection<BaseDataViewModel>();

        public LoaderStatusViewModel() : base()
        {
            MessengerInstance.Register<LoadOutputDataMessage>(this, OnLoadOutputDataMessage);
        }

        private void OnLoadOutputDataMessage(LoadOutputDataMessage msg)
        {
            DataItems.Clear();

            msg.Items.Where((o) => o.DataCategory == Registers.Models.Enums.DataCategory.Status)
                     .ToList()
                     .ForEach((o) => DataItems.Add(o));
        }
    }
}
