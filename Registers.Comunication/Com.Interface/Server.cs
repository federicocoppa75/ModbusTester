using EasyModbus;
using GalaSoft.MvvmLight.Messaging;
using Registers.Comunication.Messages;
using Registers.Comunication.Messages.ComState;
using System;
using System.Collections.Generic;

namespace Registers.Comunication.Com.Interface
{
    public class Server : IDisposable
    {
        ModbusServer _modbusServer = new ModbusServer();
        List<int> _registers = new List<int>();
        int _startIndex;
        int _bufferSize;

        private Server()
        {
            Messenger.Default.Register<WriteValueMessage>(this, OnWriteValueMessage);
            Messenger.Default.Register<WriteBitValueMessage>(this, OnWriteBitValueMessage);
            _modbusServer.HoldingRegistersChanged += OnHoldingRegistersChanged;
        }

        private void OnWriteValueMessage(WriteValueMessage msg)
        {
            if (_modbusServer != null)
            {

                var index = msg.Register - _startIndex;
                var value = _registers[index];

                if (value != msg.Value)
                {
                    _registers[index] = msg.Value;
                    _modbusServer.holdingRegisters[msg.Register + 1] = (short)msg.Value;
                }
            }
        }

        private void OnWriteBitValueMessage(WriteBitValueMessage msg)
        {
            if (_modbusServer != null)
            {
                var index = msg.Register - _startIndex;
                var value = _registers[index];
                var mask = 1 << msg.BitIndex;
                var v = value & mask;
                var b = v != 0;

                if (b != msg.Value)
                {
                    var newValue = msg.Value ? (int)value | mask : (int)value & ~mask;
                    _registers[index] = newValue;
                    _modbusServer.holdingRegisters[msg.Register + 1] = (short)newValue;
                }
            }
        }

        private void OnHoldingRegistersChanged(int register, int numberOfRegisters)
        {
            for (int i = 0; i < numberOfRegisters; i++)
            {
                OnHoldingRegisterChanged(register + i);
            }
        }

        private void OnHoldingRegisterChanged(int register)
        {
            var index = register - _startIndex - 1;
            var value = _registers[index];
            var newVal = _modbusServer.holdingRegisters[register];

            if (value != newVal)
            {
                _registers[index] = newVal;
                Messenger.Default.Send(new ValueChangedMessage() { Register = register - 1, Value = newVal });
            }
        }

        public static Server Create(int startIndex, int bufferSize)
        {
            var server = new Server();

            server.InitBuffer(startIndex, bufferSize);

            return server;
        }

        public void Start()
        {
            if(_modbusServer != null)
            {
                _modbusServer.Listen();
                Messenger.Default.Send(new ServerStateChangedMessage() { IsActive = true });
            }
        }

        public void Stop()
        {
            if(_modbusServer != null)
            {
                _modbusServer.StopListening();
                Messenger.Default.Send(new ServerStateChangedMessage() { IsActive = false });
            }

            _registers.Clear();
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
                    Stop();

                    _modbusServer = null;
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
            // TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override del finalizzatore.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
