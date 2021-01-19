using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetCareIoT.Models
{
    public class Message
    {
        public string From { get; set; }
        public string To { get; set; }
        public Data Data { get; set; }
    }
}
