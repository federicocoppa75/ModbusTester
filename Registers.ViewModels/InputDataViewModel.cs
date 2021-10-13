using GalaSoft.MvvmLight;
using Registers.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registers.ViewModels
{
    public class InputDataViewModel : BaseIODataViewModel
    {

        public InputDataViewModel() : base()
        {
            MessengerInstance.Register<LoadInputDataMessage>(this, (m) => LoadData(m.Items));
        }
    }
}
