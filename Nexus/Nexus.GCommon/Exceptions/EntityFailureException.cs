using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.GCommon.Exceptions
{
    public class EntityFailureException : Exception
    {
        public EntityFailureException()
        {

        }

        public EntityFailureException(string msg)
            : base(msg)
        {
            
        }


    }
}
