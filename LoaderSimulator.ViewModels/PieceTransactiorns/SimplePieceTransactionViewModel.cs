using System;
using System.Collections.Generic;
using System.Text;
using LoaderSimulator.ViewModels.Enums;

namespace LoaderSimulator.ViewModels.PieceTransactiorns
{
    public class SimplePieceTransactionViewModel : PieceTransactonViewModel
    {
        public override Transaction Type => Transaction.Simple;

        public SimplePieceTransactionViewModel() : base()
        {

        }
    }
}
