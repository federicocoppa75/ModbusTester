using LoaderSimulator.StateMachine.Enums;
using LoaderSimulator.ViewModels.PieceTransactiorns;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.ViewModels
{
    public class MockPieceTrandactionsViewModel : PieceTransactionsViewModel
    {
        public MockPieceTrandactionsViewModel() : base()
        {
            Transactions.Add(new SimplePieceTransactionViewModel() { Name = "Not connected state" });
            Transactions.Add(new SimplePieceTransactionViewModel() { Name = "Idle state" });
            Transactions.Add(new PieceExchangeTransactionViewModel() { Name = "Loading in position 3 (on belt)", ExchangeDirection = ExchangeDirection.Load, ExchangeType = ExchangeType.OnStop, Position = 2 });
            Transactions.Add(new NeedToConfermTransactionViewModel() { Name = "Loading in position 3 (on belt)", ActionToConferm = () => { } });
            Transactions.Add(new SimplePieceTransactionViewModel() { Name = "Idle state" });
            Transactions.Add(new PieceExchangeTransactionViewModel() { Name = "Unoading in position 4 (on belt)", ExchangeDirection = ExchangeDirection.Unload, ExchangeType = ExchangeType.OnClamp, Position = 3 });
            Transactions.Add(new SimplePieceTransactionViewModel() { Name = "Idle state" });
        }
    }
}
