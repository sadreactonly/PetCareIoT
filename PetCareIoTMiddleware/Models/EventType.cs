using System;

namespace PetCareIoTMiddleware.Models
{
    [Flags]
    public enum EventType : byte
    {
        /// <summary>
        /// Start feeding.
        /// </summary>
        FEEDER_START = 0x01,

        /// <summary>
        /// Start pump.
        /// </summary>
        PUMP_START = 0x02,

        /// <summary>
        /// Turn light on.
        /// </summary>

        LIGHT_ON = 0x03,

        /// <summary>
        /// Turn light off.
        /// </summary>
        LIGHT_OFF = 0x04,

        /// <summary>
        /// Recieved message from Arduino with water level.
        /// </summary>
        WATER_LEVEL = 0x5,

        /// <summary>
        /// Message from Arduino which represents that previous request is done successfully.
        /// </summary>
        DONE = 0x08
    }
}