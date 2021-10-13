using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.ViewModels.Enums
{
    public enum Transaction
    {
        //LoadingOnStop,          // fase di carico sulla battuta
        //LoadingOnBelt,          // fase di carico sul nastro del banco
        //UnloadingOnClamp,       // fase di scarico con scambio pannello da pinza a robot
        //UnloadingOnBelt,        // fase di scarico sul nastro del banco
        //AbortFromMachine,       // fase di abort lanciato dalla macchina
        //AbortFromLoader         // fase di abort lanciato dal robot

        Simple,
        PieceExchange,
        NeedToConferm,
        PiecePreExchange
    }
}
