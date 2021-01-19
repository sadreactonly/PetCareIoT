using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace PetCareIoT.Models
{
    [Table("Items")]

    public class ESP8266Info
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Address { get; set; }

        public bool Using { get; set; } = false;
    }
}