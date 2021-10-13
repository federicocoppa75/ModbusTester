using LoaderSimulator.ViewModels;
using System.Windows.Controls;

namespace LoaderSimulator.Views
{
    /// <summary>
    /// Logica di interazione per LoaderStatusView.xaml
    /// </summary>
    public partial class LoaderStatusView : UserControl
    {
        public LoaderStatusView()
        {
            InitializeComponent();

            DataContext = new LoaderStatusViewModel();
        }
    }
}
