using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.AssetCategoryDtos
{
    public class AssetCategoryUpdateDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryDescriptiom { get; set; }
    }
}
