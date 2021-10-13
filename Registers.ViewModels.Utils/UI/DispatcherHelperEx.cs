using GalaSoft.MvvmLight.Threading;
using Registers.Models.Interface.UI;
using System;

namespace Registers.ViewModels.Utils.UI
{
    public class DispatcherHelperEx : IDispatcherHelper
    {
        public DispatcherHelperEx()
        {

        }

        public static void Initialize() => DispatcherHelper.Initialize();

        public void CheckBeginInvokeOnUi(Action action) => DispatcherHelper.CheckBeginInvokeOnUI(action);
    }
}
