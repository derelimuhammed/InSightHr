using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T> where T : class
    {
        public T Data { get; }
        public DataResult(T data, bool isSuccess) : base(isSuccess) => this.Data = data;
        public DataResult(T data, bool isSuccess, string message) : base(isSuccess, message) => this.Data = data;

    }
}
