using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Registers.ViewModels
{
    public class BaseIODataViewModel : ViewModelBase
    {
        public ObservableCollection<BaseDataViewModel> DataItems { get; set; } = new ObservableCollection<BaseDataViewModel>();

        private BaseDataViewModel _selected;

        public BaseDataViewModel Selected
        {
            get => _selected;
            set => Set(ref _selected, value, nameof(Selected));
        }


        public BaseIODataViewModel() : base()
        {

        }

        protected void LoadData(IEnumerable<BaseDataViewModel> items)
        {
            DataItems.Clear();

            foreach (var item in items)
            {
                DataItems.Add(item);
            }
        }
    }
}
