using LoaderSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoaderSimulator.Views
{
    /// <summary>
    /// Logica di interazione per PieceTransactionsView.xaml
    /// </summary>
    public partial class PieceTransactionsView : UserControl
    {
        CollectionView _view;

        public PieceTransactionsView()
        {
            InitializeComponent();

            DataContext = new PieceTransactionsViewModel();
            //DataContext = new MockPieceTrandactionsViewModel();

            _view = (CollectionView)CollectionViewSource.GetDefaultView((DataContext as PieceTransactionsViewModel).Transactions);
            _view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
        }
    }
}
