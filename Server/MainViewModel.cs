using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Registers.Models;
using Registers.Models.Enums;
using Registers.Utils.Extensions;
using Registers.ViewModels;
using Registers.ViewModels.Messages;
using System.Linq;
using System.Windows.Input;
using ComInterface = Registers.Comunication.Com.Interface;

namespace Server
{
    class MainViewModel : ViewModelBase
    {
        ComInterface.Server _sever;

        int _startIndex = -1;
        int _bufferSize = -1;

        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand { get { return _fileOpenCommand ?? (_fileOpenCommand = new RelayCommand(() => FileOpenCommandImplementation())); } }

        private ICommand _comunicationStartCommand;
        public ICommand ComunicationStartCommand { get { return _comunicationStartCommand ?? (_comunicationStartCommand = new RelayCommand(() => ComunicationStartCommandImplementation(), () => _sever == null)); } }

        private ICommand _comunicationStopCommand;
        public ICommand ComunicationStopCommand { get { return _comunicationStopCommand ?? (_comunicationStopCommand = new RelayCommand(() => ComunicationStopCommandImplementation(), () => _sever != null)); } }

        private ICommand _commandsStartClockCommand;
        public ICommand CommandsStartClockCommand { get { return _commandsStartClockCommand ?? (_commandsStartClockCommand = new RelayCommand(() => CommandsStartClockCommandImplementation(), () => _sever != null)); } }

        private ICommand _commandsStopClockCommand;
        public ICommand CommandsStopClockCommand { get { return _commandsStopClockCommand ?? (_commandsStopClockCommand = new RelayCommand(() => CommandsStopClockCommandImplementation(), () => _sever != null)); } }

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

                    if (bd.DataItems.Count > 0)
                    {

                        var inputData = bd.DataItems.Where((o) => o.DataDirection == DataDirection.Input)
                                                    .Select((o) => o.ToViewModel<BaseDataViewModel>())
                                                    .ToList();
                        var outputData = bd.DataItems.Where((o) => o.DataDirection == DataDirection.Output)
                                                     .Select((o) => o.ToViewModel<BaseDataViewModel>())
                                                     .ToList();

                        MessengerInstance.Send(new LoadInputDataMessage() { Items = inputData });
                        MessengerInstance.Send(new LoadOutputDataMessage() { Items = outputData });

                        var indexes = bd.DataItems.Select((o) => o.Register).ToList();
                        _startIndex = indexes.Min();
                        _bufferSize = indexes.Max() - _startIndex + 1;
                    }
                }
            }
        }

        private void ComunicationStartCommandImplementation()
        {
            if (_sever == null) _sever = ComInterface.Server.Create(_startIndex, _bufferSize);

            _sever.Start();
        }

        private void ComunicationStopCommandImplementation()
        {
            if(_sever != null)
            {
                _sever.Stop();
                _sever.Dispose();
                _sever = null;
            }
        }

        private void CommandsStartClockCommandImplementation() => MessengerInstance.Send(new StartClockMessage() { Direction = DataDirection.Output });

        private void CommandsStopClockCommandImplementation() => MessengerInstance.Send(new StopClockMessage());

        public override void Cleanup()
        {
            if (_sever != null) _sever.Dispose();
            base.Cleanup();
        }
    }
}
