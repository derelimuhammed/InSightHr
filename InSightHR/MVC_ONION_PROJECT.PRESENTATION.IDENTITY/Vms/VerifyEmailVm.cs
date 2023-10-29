namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Vms
{
    public class VerifyEmailVm
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string NewPassword { get; set; }
        public string CorfirmPassword { get; set; }
    }
}
