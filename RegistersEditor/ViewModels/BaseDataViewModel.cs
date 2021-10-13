using GalaSoft.MvvmLight;
using Registers.Models.Enums;
using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistersEditor.ViewModels
{
    public class BaseDataViewModel : ViewModelBase, IBaseData
    {
        private string _name;

        public string Name
        {
            get => _name; 
            set => Set(ref _name, value, nameof(Name)); 
        }

        private int _register;

        public int Register
        {
            get => _register; 
            set => Set(ref _register, value, nameof(Register));
        }

        private DataDirection _dataDirection;

        public DataDirection DataDirection
        {
            get => _dataDirection; 
            set => Set(ref _dataDirection, value, nameof(DataDirection));
        }

        private DataCategory _dataCategory;

        public DataCategory DataCategory
        {
            get => _dataCategory;
            set => Set(ref _dataCategory, value, nameof(DataCategory));
        }


        public BaseDataViewModel() : base()
        {

        }
    }
}
