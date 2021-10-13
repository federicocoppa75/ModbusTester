using EasyModbus;
using GalaSoft.MvvmLight.Messaging;
using Registers.Comunication.Messages;
using Registers.Comunication.Messages.ComState;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace Registers.Comunication.Com.Interface
{
    public class Client : IDisposable
    {
        int _timeStamp;
        Timer _timer;
        ModbusClient _modbusClient;
        List<int> _registers = new List<int>();
        int _startIndex;
        int _bufferSize;
        object _lockObj = new object();

        private Client(string ipAddress, int port, int timeStamp)
        {
            _timeStamp = timeStamp;
            _timer = new Timer(_timeStamp);
            _modbusClient = new ModbusClient(ipAddress, port);
            _modbusClient.ConnectedChanged += OnConnectedChanged;
            //_modbusClient.ReceiveDataChanged += OnReceiveDataChanged;
            //_modbusClient.SendDataChanged += OnSendDataChanged;

            Messenger.Default.Register<WriteValueMessage>(this, OnWriteValueMessage);
            Messenger.Default.Register<WriteBitValueMessage>(this, OnWriteBitValueMessage);
        }

        private void OnConnectedChanged(object sender) => Messenger.Default.Send(new ConnectionToServerChangedMessage() { IsActive = _modbusClient.Connected });

        //private void OnSendDataChanged(object sender)
        //{
        //}

        //private void OnReceiveDataChanged(object sender)
        //{
        //}

        public static Client Create(string ipAddress, int port, int timeStamp, int startIndex, int bufferSize)
        {
            var client = new Client(ipAddress, port, timeStamp);

            client.InitBuffer(startIndex, bufferSize);

            return client;
        }

        public void Connect()
        {
            _modbusClient.Connect();

            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        public void Disconnect()
        {
            if (_modbusClient != null && _modbusClient.Connected)
            {
                _modbusClient.Disconnect();

            }

            if(_timer != null)
            {
                _timer.Enabled = false;
                _timer.Elapsed -= OnTimerElapsed;
            }

            _registers.Clear();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if ((_modbusClient != null) && _modbusClient.Connected)
            {
                Task.Run(() =>
                {
                    _timer.Enabled = false;

                    lock (_lockObj)
                    {
                        if(ReadRegisters(_startIndex, _bufferSize, out int[] values))
                        {
                            for (int i = 0; i < _bufferSize; i++)
                            {
                                var index = _startIndex + i;
                                var rv = values[i];
                                var v = _registers[i];

                                if (rv != v)
                                {
                                    _registers[i] = rv;
                                    Messenger.Default.Send(new ValueChangedMessage() { Register = index, Value = rv });
                                }
                            }
                        }                       
                    }

                    _timer.Enabled = true;
                });

            }
        }
        
        private void OnWriteValueMessage(WriteValueMessage msg)
        {
            if ((_modbusClient != null) && _modbusClient.Connected)
            {
                Task.Run(() =>
                {
                    var index = msg.Register - _startIndex;

                    lock (_lockObj)
                    {
                        var value = _registers[index];

                        if (value != msg.Value)
                        {                            
                            if(WriteRegister(msg.Register, msg.Value)) _registers[index] = msg.Value;
                        }
                    }
                });
            }
        }

        private void OnWriteBitValueMessage(WriteBitValueMessage msg)
        {
            if ((_modbusClient != null) && _modbusClient.Connected)
            {
                Task.Run(() =>
                {
                    var index = msg.Register - _startIndex;
                    var mask = 1 << msg.BitIndex;

                    lock (_lockObj)
                    {
                        var value = _registers[index];
                        var v = value & mask;
                        var b = v != 0;

                        if (b != msg.Value)
                        {
                            var newValue = msg.Value ? (int)value | mask : (int)value & ~mask;
                            
                            if(WriteRegister(msg.Register, newValue)) _registers[index] = newValue;
                        }
                    }
                });
            }
        }

        private bool WriteRegister(int register, int value)
        {
            bool result = false;

            try
            {
                _modbusClient.WriteSingleRegister(register, value);
                result = true;
            }
            catch (Exception e)
            {
            }

            return result;
        }

        private bool ReadRegisters(int startAddress, int size, out int[] values)
        {
            bool result = false;

            try
            {
                values = _modbusClient.ReadHoldingRegisters(_startIndex, _bufferSize);
                result = true;
            }
            catch (Exception)
            {

                values = new int[0];
            }

            return result;
        }

        private void InitBuffer(int startIndex, int bufferSize)
        {
            _startIndex = startIndex;
            _bufferSize = bufferSize;

            for (int i = 0; i < bufferSize; i++) _registers.Add(0);
        }


        #region IDisposable Support
        private bool disposedValue = false; // Per rilevare chiamate ridondanti

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminare lo stato gestito (oggetti gestiti).
                    Disconnect();

                    if (_modbusClient != null)
                    {
                        //_modbusClient.ReceiveDataChanged -= OnReceiveDataChanged;
                        //_modbusClient.SendDataChanged -= OnSendDataChanged;
                        _modbusClient.ConnectedChanged -= OnConnectedChanged;
                        _modbusClient = null;
                    }

                    if(_timer != null)
                    {
                        _timer.Dispose();
                        _timer = null;
                    }

                }

                // TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di un finalizzatore.
                // TODO: impostare campi di grandi dimensioni su Null.

                disposedValue = true;
            }
        }

        // Questo codice viene aggiunto per implementare in modo corretto il criterio Disposable.
        public void Dispose()
        {
            // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
            Dispose(true);
        }

        #endregion
    }
}
