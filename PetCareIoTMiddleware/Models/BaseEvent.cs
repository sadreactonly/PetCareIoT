using System;

namespace PetCareIoTMiddleware.Models
{
    /// <summary>
    /// Class representation of project's events.
    /// </summary>
    public class BaseEvent
    {
        public BaseEvent()
        {
        }

        /// <summary>
        /// Creates new BaseEvent Object.
        /// </summary>
        /// <param name="type"> Type of event (feeding, watering, etc.)</param>
        /// <param name="guid"> Global identificator of event. </param>
        public BaseEvent(EventType type, string guid)
        {
            Type = type;
            Guid = guid;
            Start = DateTime.UtcNow;
            End = DateTime.MinValue;
            Done = false;
        }

        /// <summary>
        /// Represents identificator.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents global identificator.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Represents event's start date and tame.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Represents event's start date and tame. MinValue if is not endend.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Represents event's state.
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// Represents event type.
        /// </summary>
        public EventType Type { get; set; }
    }
}