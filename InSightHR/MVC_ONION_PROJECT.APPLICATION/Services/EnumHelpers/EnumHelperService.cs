using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.EnumHelpers
{
    public class EnumHelperService:IEnumHelperService
    {
        public string GetDisplayName(Enum value)
        {
            var displayAttribute = value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .SingleOrDefault() as DisplayAttribute;

            return displayAttribute?.Name ?? value.ToString();
        }
    }
}
