using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Registers.Models.Interface;
using Registers.ViewModels.Enums;
using Registers.ViewModels.Messages;

namespace Registers.ViewModels
{
    public class ClockDataViewModel : BitDataViewModel, IClockData, IBitData, IBaseData
    {
        Timer _timer;

        public override DataType DataType => DataType.Clock;

        public int Period { get; set; }

        public ClockDataViewModel() : base()
        {
            MessengerInstance.Register<StartClockMessage>(this, OnStartClockMessage);
            MessengerInstance.Register<StopClockMessage>(this, OnStopClockMessage);
        }

        private void OnStopClockMessage(StopClockMessage msg) => StopClock();

        private void OnStartClockMessage(StartClockMessage msg)
        {
            if (msg.Direction == DataDirection)
            {
                _timer = new Timer(Period / 2);
                _timer.Elapsed += OnTimerElapsed;
                _timer.AutoReset = true;
                _timer.Enabled = true;
            }
        }

        private void StopClock()
        {
            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Elapsed -= OnTimerElapsed;
                _timer.Dispose();
                _timer = null;
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => Value = !Value;

        public override void Cleanup()
        {
            if(_timer != null)
            {
                StopClock();
            }

            base.Cleanup();
        }

    }
}
