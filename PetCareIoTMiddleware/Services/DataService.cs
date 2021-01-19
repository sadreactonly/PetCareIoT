using PetCareIoTMiddleware.Authentication;
using PetCareIoTMiddleware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetCareIoTMiddleware.Services
{
    /// <summary>
    /// Class responsible for managing data.
    /// </summary>
    public class DataService
    {
        private readonly ApplicationDbContext _context;
        private readonly object balanceLock = new object();

        public DataService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get last state of light relay.
        /// </summary>
        /// <returns></returns>
        public int GetLastLightState()
        {
            int lastState = 0;
            var lastLightEvent = _context.Events.OrderBy(x => x.Start).Where(x => x.End != DateTime.MinValue).FirstOrDefault();
            if (lastLightEvent.Type == EventType.LIGHT_ON)
            {
                lastState = 1;
            }

            return lastState;
        }

        /// <summary>
        /// Get all events of type Light
        /// </summary>
        /// <returns></returns>
        public List<BaseEvent> GetAllLightEvents()
        {
            return _context.Events.Where(x => x.Type == EventType.LIGHT_OFF || x.Type == EventType.LIGHT_ON).ToList();
        }

        /// <summary>
        /// Get all events of type Pump
        /// </summary>
        /// <returns></returns>
        public List<BaseEvent> GetAllPumpEvents()
        {
            return _context.Events.Where(x => x.Type == EventType.PUMP_START).ToList();
        }

        /// <summary>
        /// Get all events of type Feeder
        /// </summary>
        /// <returns></returns>
        public List<BaseEvent> GetAllFeederEvents()
        {
            return _context.Events.Where(x => x.Type == EventType.FEEDER_START).ToList();
        }

        /// <summary>
        /// Add event to database.
        /// </summary>
        /// <param name="e"></param>
        public void AddEvent(BaseEvent e)
        {
            try
            {
                lock (balanceLock)
                {
                    _context.Events.Add(e);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Update existing event.
        /// </summary>
        /// <param name="gid"></param>
        public void UpdateEvent(string gid)
        {
            lock (balanceLock)
            {
                if (_context.Events.Count(e => e.Guid == gid && e.Done == false) > 0)
                {
                    try
                    {
                        var _event = _context.Events.Where(e => e.Guid == gid).First();
                        _event.End = DateTime.UtcNow;
                        _event.Done = true;

                        _context.Events.Update(_event);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
    }
}