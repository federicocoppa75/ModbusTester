using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.ViewModels.PieceTransactiorns
{
    public class PiecePreExchangeTransactionViewModel : PieceTransactonViewModel
    {
        public override Transaction Type => Transaction.PiecePreExchange;
        public int Position { get; set; }
        public ExchangeType ExchangeType { get; set; }
        public ExchangeDirection ExchangeDirection { get; set; }

        public PiecePreExchangeTransactionViewModel() : base()
        {

        }
    }
}
