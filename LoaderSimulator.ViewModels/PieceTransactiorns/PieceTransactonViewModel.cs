using GalaSoft.MvvmLight;
using LoaderSimulator.ViewModels.Enums;
using Registers.Models.Interface;
using System;
using System.Collections.Generic;
using System.Text;


namespace LoaderSimulator.ViewModels.PieceTransactiorns
{
    public abstract class PieceTransactonViewModel : ViewModelBase, IPropertyEquatable<Transaction>
    {
        static int _seed;

        public int Id { get; private set; }

        public abstract Transaction Type { get; }

        public string Name { get; set; }
        public string AdditionalInfos { get; set; }


        public PieceTransactonViewModel() : base()
        {
            Id = ++_seed;
        }

        public bool PropEquals(Transaction value) => value == Type;

    }
}
