using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using LoaderSimulator.StateMachine;
using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.StateMachine.Interfaces;
using LoaderSimulator.StateMachine.Messages;
using LoaderSimulator.ViewModels.Helpers.UI;
using LoaderSimulator.ViewModels.Interfaces;
using LoaderSimulator.ViewModels.PieceTransactiorns;
using Registers.Models.Interface;
using Registers.ViewModels;
using Registers.ViewModels.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LoaderSimulator.ViewModels
{
    public class PieceTransactionsViewModel : ViewModelBase, IContext
    {        
        ISignalsSelectors _signalsSelectors;

        public ObservableCollection<BaseDataViewModel> DataItems { get; set; } = new ObservableCollection<BaseDataViewModel>();
        public ObservableCollection<PieceTransactonViewModel> Transactions { get; set; } = new ObservableCollection<PieceTransactonViewModel>();

        ICommand _abortCommand;
        public ICommand AbortCommand => _abortCommand ?? (_abortCommand = new RelayCommand(() => AbortCommandImplementation(), () => _state is IAbortable));

        public string Name { get; set; } = "Pippo";

        public PieceTransactionsViewModel() : base()
        {
            _signalsSelectors = SimpleIoc.Default.GetInstance<ISignalsSelectors>();

            MessengerInstance.Register<LoadAllDataMessage>(this, OnLoadAllDataMessage);
            MessengerInstance.Register<GetSignalForStartLoadMessage>(this, OnGetSignalForLoadMessage);
            MessengerInstance.Register<GetSignalForStartUnloadMessage>(this, OnGetSignalForStartUnloadMessage);
            MessengerInstance.Register<GetSignalsForPieceExchangeMessage>(this, OnGetSignalsForPieceExchangeMessage);
            MessengerInstance.Register<GetSignalMessage>(this, OnGetSignalMessage);

            State = new NotConnectedState() { Context = this };
            State.Start();
        }

        private void OnGetSignalMessage(GetSignalMessage msg) => _signalsSelectors.GetSignal(msg);

        private void OnGetSignalsForPieceExchangeMessage(GetSignalsForPieceExchangeMessage msg) => _signalsSelectors.GetSignalsForPieceExchange(msg);

        private void OnGetSignalForStartUnloadMessage(GetSignalForStartUnloadMessage msg) => _signalsSelectors.GetSignalForUnload(msg);

        private void OnGetSignalForLoadMessage(GetSignalForStartLoadMessage msg) => _signalsSelectors.GetSignalForLoad(msg);


        private void OnLoadAllDataMessage(LoadAllDataMessage msg)
        {
            DataItems.Clear();

            msg.Items.ForEach((o) => DataItems.Add(o));
        }

        #region IContext

        IState _state;
        public IState State
        {
            get => _state;
            set
            {
                if (Set(ref _state, value, nameof(State)))
                {
                    OnStateChanged();
                }
            }
        }

        public void LoadPanel(int loadPosition, ExchangeType exchangeType)
        {
            DispatcherHelperEx.CheckBeginInvokeOnUI(() =>
            {
                Transactions.Add(new PieceExchangeTransactionViewModel()
                {
                    Name = State.Name,
                    ExchangeType = exchangeType,
                    Position = loadPosition,
                    ExchangeDirection = ExchangeDirection.Load,
                    AdditionalInfos = GetLoadPanelAdditionalInfo()
                });
                UpdateAbortCommandCanExecute();
            });
        }

        public void PreLoadPanel(int loadPosition, ExchangeType exchangeType)
        {
            DispatcherHelperEx.CheckBeginInvokeOnUI(() =>
            {
                Transactions.Add(new PiecePreExchangeTransactionViewModel()
                {
                    Name = "Preload State",
                    ExchangeType = exchangeType,
                    Position = loadPosition,
                    ExchangeDirection = ExchangeDirection.Load,
                    AdditionalInfos = GetLoadPanelAdditionalInfo()
                });
                UpdateAbortCommandCanExecute();
            });
        }

        public void RequestLoadPanelExcutionConferm(int loadPosition, ExchangeType exchangeType, Action confermAction)
        {
            DispatcherHelperEx.CheckBeginInvokeOnUI(() =>
            {
                Transactions.Add(new NeedToConfermTransactionViewModel()
                {
                    Name = State.Name,
                    ActionToConferm = confermAction
                });
                UpdateAbortCommandCanExecute();
            });
        }

        public void UnloadPanel(int loadPosition, ExchangeType exchangeType)
        {
            DispatcherHelperEx.CheckBeginInvokeOnUI(() =>
            {
                Transactions.Add(new PieceExchangeTransactionViewModel()
                {
                    Name = State.Name,
                    ExchangeType = exchangeType,
                    Position = loadPosition,
                    ExchangeDirection = ExchangeDirection.Unload,
                    AdditionalInfos = GetLoadPanelAdditionalInfo()
                });
                UpdateAbortCommandCanExecute();
            });
        }

        public void PreUnloadPanel(int loadPosition, ExchangeType exchangeType)
        {
            DispatcherHelperEx.CheckBeginInvokeOnUI(() =>
            {
                Transactions.Add(new PiecePreExchangeTransactionViewModel()
                {
                    Name = "Preunload State",
                    ExchangeType = exchangeType,
                    Position = loadPosition,
                    ExchangeDirection = ExchangeDirection.Unload,
                    AdditionalInfos = GetLoadPanelAdditionalInfo()
                });
                UpdateAbortCommandCanExecute();
            });
        }

        public void RequestMachineAbortAck(Action ackAction)
        {
            DispatcherHelperEx.CheckBeginInvokeOnUI(() => 
            {
                Transactions.Add(new NeedToConfermTransactionViewModel()
                {
                    Name = State.Name,
                    ActionToConferm = ackAction
                });
                UpdateAbortCommandCanExecute();
            });
        }

        //public void OnStatusChanged()
        //{
        //    throw new NotImplementedException();
        //}

        public void Log(LogType type, string message)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void OnStateChanged()
        {
            DispatcherHelperEx.CheckBeginInvokeOnUI(() =>
            {
                Transactions.Add(new SimplePieceTransactionViewModel() 
                { 
                    Name = _state.Name,
                    AdditionalInfos = string.Empty //GetLoadPanelAdditionalInfo()
                });
                UpdateAbortCommandCanExecute();
            });

        }
        
        private void AbortCommandImplementation() => (_state as IAbortable).Abort();

        private void UpdateAbortCommandCanExecute() => (AbortCommand as RelayCommand).RaiseCanExecuteChanged();

        private string GetLoadPanelAdditionalInfo()
        {            
            var programDatas = DataItems.Where(o => (o.DataCategory == Registers.Models.Enums.DataCategory.ProgramData) && (o is ValueDataViewModel)).Cast<ValueDataViewModel>().ToList();

            if((programDatas != null) && (programDatas.Count > 0))
            {
                var sb = new StringBuilder();

                foreach (var item in programDatas)
                {
                    sb.AppendFormat($"{item.Value} ");
                }

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }            
        }

    }
}
