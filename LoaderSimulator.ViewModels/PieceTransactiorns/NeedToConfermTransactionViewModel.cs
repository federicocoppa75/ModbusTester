using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using LoaderSimulator.ViewModels.Enums;

namespace LoaderSimulator.ViewModels.PieceTransactiorns
{
    public class NeedToConfermTransactionViewModel : PieceTransactonViewModel
    {
        public override Transaction Type => Transaction.NeedToConferm;
        public Action ActionToConferm { get; set; }

        ICommand _confermCommand;
        public ICommand ConfermCommand => _confermCommand ?? (_confermCommand = new RelayCommand(() => ActionToConferm?.Invoke()));

        public NeedToConfermTransactionViewModel() : base()
        {

        }
    }
}
