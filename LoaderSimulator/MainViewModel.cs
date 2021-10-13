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

namespace LoaderSimulator
{
    class MainViewModel : ViewModelBase
    {
        ComInterface.Client _client;      

        int _startIndex = -1;
        int _bufferSize = -1;

        private string _ipAddress = "127.0.0.1";

        public string IpAddress
        {
            get => _ipAddress;
            set => Set(ref _ipAddress, value, nameof(IpAddress));
        }

        private int _port = 502;

        public int Port
        {
            get => _port;
            set => Set(ref _port, value, nameof(Port));
        }

        private int _timeStamp = 200;

        public int TimeStamp
        {
            get => _timeStamp;
            set => Set(ref _timeStamp, value, nameof(TimeStamp));
        }

        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand { get { return _fileOpenCommand ?? (_fileOpenCommand = new RelayCommand(() => FileOpenCommandImplementation())); } }

        private ICommand _comunicationConnectCommand;
        public ICommand ComunicationConnectCommand { get { return _comunicationConnectCommand ?? (_comunicationConnectCommand = new RelayCommand(() => ComunicationConnectCommandImplementation(), () => _client == null)); } }

        private ICommand _comunicationDisconnectCommand;
        public ICommand ComunicationDisconnectCommand { get { return _comunicationDisconnectCommand ?? (_comunicationDisconnectCommand = new RelayCommand(() => ComunicationDisconnectCommandImplementation(), () => _client != null)); } }

        private ICommand _commandsStartClockCommand;
        public ICommand CommandsStartClockCommand { get { return _commandsStartClockCommand ?? (_commandsStartClockCommand = new RelayCommand(() => CommandsStartClockCommandImplementation(), () => _client != null)); } }

        private ICommand _commandsStopClockCommand;
        public ICommand CommandsStopClockCommand { get { return _commandsStopClockCommand ?? (_commandsStopClockCommand = new RelayCommand(() => CommandsStopClockCommandImplementation(), () => _client != null)); } }


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
                        var allData = bd.DataItems.Select((o) => o.ToViewModel<BaseDataViewModel>())
                                                  .ToList();

                        MessengerInstance.Send(new LoadInputDataMessage() { Items = outputData });
                        MessengerInstance.Send(new LoadOutputDataMessage() { Items = inputData });
                        MessengerInstance.Send(new LoadAllDataMessage() { Items = allData });

                        var indexes = bd.DataItems.Select((o) => o.Register).ToList();
                        _startIndex = indexes.Min();
                        _bufferSize = indexes.Max() - _startIndex + 1;
                    }
                }
            }
        }

        private void ComunicationConnectCommandImplementation()
        {
            if (_client == null) _client = ComInterface.Client.Create(_ipAddress, _port, _timeStamp, _startIndex, _bufferSize);

            _client.Connect();
        }


        private void ComunicationDisconnectCommandImplementation()
        {
            if (_client != null)
            {
                _client.Disconnect();
                _client.Dispose();
                _client = null;
            }
        }

        private void CommandsStartClockCommandImplementation() => MessengerInstance.Send(new StartClockMessage() { Direction = DataDirection.Input });

        private void CommandsStopClockCommandImplementation() => MessengerInstance.Send(new StopClockMessage());

        public override void Cleanup()
        {
            if (_client != null) _client.Dispose();
            base.Cleanup();
        }
    }
}
