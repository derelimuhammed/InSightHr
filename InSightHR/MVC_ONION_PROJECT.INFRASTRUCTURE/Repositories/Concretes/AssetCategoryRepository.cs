using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.INFRASTRUCTURE.APPCONTEXT;
using MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.EntityFramework;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Concretes
{
    public class AssetCategoryRepository : BaseRepository<AssetCategory>, IAssetCategoryRepository
    {
        public AssetCategoryRepository(AppDBContext context) : base(context)
        {
        }
    }
}
