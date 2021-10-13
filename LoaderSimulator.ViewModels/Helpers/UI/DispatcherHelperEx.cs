using GalaSoft.MvvmLight.Ioc;
using Registers.Models.Interface.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.ViewModels.Helpers.UI
{
    public static class DispatcherHelperEx
    {
        private static IDispatcherHelper _dispatcherHelper = null;

        public static void CheckBeginInvokeOnUI(Action action)
        {
            var dispatcherHelper = (_dispatcherHelper ?? SimpleIoc.Default.GetInstance<IDispatcherHelper>());

            dispatcherHelper.CheckBeginInvokeOnUi(action);
        }
    }
}
