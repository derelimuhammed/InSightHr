using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.Utilities
{
    public interface IResult
    {
        bool IsSuccess { get; }
        string Message { get; }
    }
}
