using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.Package
{
    public class PackageCreateVm
    {
        [DisplayName("Paket Adı")]
        public string PackageName { get; set; }

        [DisplayName("Fiyat")]
        public double Price { get; set; }

        [DisplayName("Kullanıcı Sayısı")]
        public int NumberOfUser { get; set; }

        [DisplayName("Paket Süresi (Ay)")]
        public double PackageDurationMonth { get; set; }
    }
}
