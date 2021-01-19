

using System;

namespace PetCareIoT.Models
{
    [Flags]
    public enum EventType : byte
    {
        FEEDER_START = 0x01,
        PUMP_START = 0x02,
        LIGHT_ON = 0x03,
        LIGHT_OFF = 0x04,
        FEEDER_DONE = 0x05,
        PUMP_DONE = 0x06,
        LIGHT_DONE = 0x07,
        DONE = 0x08
    }
}