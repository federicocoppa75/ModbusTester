using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Registers.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Registers.Utils.Extensions;

namespace RegistersEditor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<BaseDataViewModel> DataItems { get; set; } = new ObservableCollection<BaseDataViewModel>();

        private BaseDataViewModel _selected;

        public BaseDataViewModel Selected
        {
            get => _selected; 
            set => Set(ref _selected, value, nameof(Selected));
        }

        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand { get { return _fileOpenCommand ?? (_fileOpenCommand = new RelayCommand(() => FileOpenCommandImplementation())); } }

        private ICommand _fileSaveCommand;
        public ICommand FileSaveCommand { get { return _fileSaveCommand ?? (_fileSaveCommand = new RelayCommand(() => FileSaveCommandImplementation())); } }

        private ICommand _editAddValueCommand;
        public ICommand EditAddValueCommand { get { return _editAddValueCommand ?? (_editAddValueCommand = new RelayCommand(() => EditAddValueCommandImplementation())); } }

        private ICommand _editAddBitCommand;
        public ICommand EditAddBitCommand { get { return _editAddBitCommand ?? (_editAddBitCommand = new RelayCommand(() => EditAddBitCommandImplementation())); } }

        private ICommand _editAddClockCommand;
        public ICommand EditAddClockCommand { get { return _editAddClockCommand ?? (_editAddClockCommand = new RelayCommand(() => EditAddClockCommandImplementation())); } }

        private ICommand _editRemoveCommand;
        public ICommand EditRemoveCommand { get { return _editRemoveCommand ?? (_editRemoveCommand = new RelayCommand(() => EditRemoveCommandImplementation(), () => Selected != null)); } }

        public MainViewModel() : base()
        {

        }

        private void FileOpenCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "mbdata", AddExtension = true, Filter = "Modbus data|*.mbdata" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DataSet));

                using (var reader = new System.IO.StreamReader(dlg.FileName))
                {
                    var bd = (DataSet)serializer.Deserialize(reader);

                    DataItems.Clear();

                    foreach (var item in bd.DataItems)
                    {
                        DataItems.Add(item.ToViewModel<BaseDataViewModel>());
                    }
                }
            }
        }

        private void FileSaveCommandImplementation()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = "mbdata";
            dlg.AddExtension = true;
            dlg.Filter = "Modbus data|*.mbdata";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DataSet));

                using (var writer = new System.IO.StreamWriter(dlg.FileName))
                {
                    var dataItems = DataItems.Select((o) => o.ToModel())
                                             .ToList();
                    var ds = new DataSet() { DataItems = dataItems };

                    serializer.Serialize(writer, ds);
                }
            }
        }

        private void EditAddValueCommandImplementation()
        {
            DataItems.Add(new ValueDataViewModel() { Register = -1, Name = "Unnamed" });
        }

        private void EditAddBitCommandImplementation()
        {
            DataItems.Add(new BitDataViewModel() { Register = -1, BitIndex = -1, Name = "Unnamed" });
        }

        private void EditAddClockCommandImplementation()
        {
            DataItems.Add(new ClockDataViewModel() { Register = -1, BitIndex = -1, Period = -1, Name = "Unamed" });
        }

        private void EditRemoveCommandImplementation()
        {
            DataItems.Remove(Selected);
            Selected = null;
        }

    }
}
