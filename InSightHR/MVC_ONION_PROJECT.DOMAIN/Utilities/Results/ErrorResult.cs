using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.Utilities.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult() : base(false)
        {

        }
        public ErrorResult(string message) : base(false, message)
        {

        }
    }
}
