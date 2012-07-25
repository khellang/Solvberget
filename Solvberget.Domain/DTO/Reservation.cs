﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Reservation
    {

        public string DocumentNumber { get; set; }
        public string DocumentTitle { get; set;  }
        public string PickupLocation { get; set; }
        public string HoldRequestFrom { get; set; }
        public string HoldRequestTo { get; set; }
        public string Status { get; set; }
    }

}
