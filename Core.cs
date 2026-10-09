using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    internal class Core
    {
        public static ScooterRentalEntities Context = new ScooterRentalEntities();

        public static User user { get; set; }

    }
}
