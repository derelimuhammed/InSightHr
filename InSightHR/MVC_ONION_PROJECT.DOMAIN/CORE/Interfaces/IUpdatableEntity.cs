using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.CORE.Interfaces
{
    public interface IUpdatableEntity: IEntity, ICreatableEntity
    {
        string? UpdatedBy { get; set; } 
        DateTime? UpdatedDate { get; set; }
    }
}
