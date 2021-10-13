using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderSimulator.StateMachine.Enums
{
    public enum Signals
    {
        // base
        SCM_ABORT_REQ,
        SCM_ABORT_ACK,
        EX_ABORT_REQ,
        EX_ABORT_ACK,
        SCM_PHOTOCELL,
        EX_NO_INTERFERENCE,
        // load
        SCM_PRELOAD_REQ,
        SCM_LOAD_REQ,
        SCM_PIECE_TAKEN,
        EX_PIECE_READY,
        EX_LOAD_END,
        // unload
        SCM_PREUNLOAD_REQ,
        SCM_UNLOAD_REQ,
        EX_UNLOAD_END,
        // unload on clamp
        SCM_PIECE_GIVEN,
        EX_PIECE_REQ
    }
}
