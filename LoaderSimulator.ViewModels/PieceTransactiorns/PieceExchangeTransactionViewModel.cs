using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.ViewModels.PieceTransactiorns
{
    public class PieceExchangeTransactionViewModel : PieceTransactonViewModel
    {
        public override Transaction Type => Transaction.PieceExchange;
        public int Position { get; set; }
        public ExchangeType ExchangeType { get; set; }
        public ExchangeDirection ExchangeDirection { get; set; }

        public PieceExchangeTransactionViewModel() : base()
        {

        }
    }
}
