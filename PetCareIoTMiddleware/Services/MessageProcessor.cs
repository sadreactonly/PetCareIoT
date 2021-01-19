using PetCareIoTMiddleware.Models;

namespace PetCareIoTMiddleware.Services
{
    /// <summary>
    /// Proccess message for saving in database.
    /// </summary>
    public class MessageProcessor
    {
        private readonly DataService _service;

        public MessageProcessor(DataService service)
        {
            _service = service;
        }

        /// <summary>
        /// Send data for saving based on type.
        /// </summary>
        /// <param name="data"> Recieved data for saving.</param>
        public void ProcessData(Data data)
        {
            switch (data.Type)
            {
                case EventType.FEEDER_START:
                case EventType.PUMP_START:
                case EventType.LIGHT_ON:
                case EventType.LIGHT_OFF:
                    _service.AddEvent(new BaseEvent(data.Type, data.RequestId));
                    break;

                case EventType.DONE:
                    _service.UpdateEvent(data.RequestId);
                    break;
            }
            return;
        }
    }
}