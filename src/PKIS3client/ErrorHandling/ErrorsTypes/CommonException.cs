using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming_of_corporate_industrial_systems_Practice_2_client.ErrorHandling.ErrorsTypes
{
    public class CommonException : Exception
    {
        public CommonException(string message) : base(message)
        {
        }
    }
}
