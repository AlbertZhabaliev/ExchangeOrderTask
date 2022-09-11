using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeOrderSelecor.Models.Enums
{
    public enum OrderStatus
    {
        //Not in use in the current version
        Executed = 0,
        NotEnoughMoney =1,
        NotEnoughBuyers=2,
        NotFilled =3,
        ConnectionError =4

    }
}
