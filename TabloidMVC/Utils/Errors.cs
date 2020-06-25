using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Utils
{
    public class Errors
    {
        public class AdministratorTypeException: Exception
        {
            public AdministratorTypeException(string message)
                :base(message)
            {

            }
        }
    }
}
