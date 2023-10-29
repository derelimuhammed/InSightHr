using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.Utilities
{
    public interface IDataResult<T> : IResult where T : class
    {
        T Data { get; }
    }
}
