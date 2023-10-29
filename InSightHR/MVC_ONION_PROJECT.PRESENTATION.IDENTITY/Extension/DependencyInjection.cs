using AspNetCoreHero.ToastNotification;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Extension
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddPresentationService(this IServiceCollection services)
        {
            //View'da soru işareti konulmayan yerler boş geçilemez anlamına geliyor. Artık soru işareti koymaya gerek olmadan
            //null izni veriyoruz aşağıdaki kodla
            services.AddControllersWithViews(opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
            services.AddAutoMapper(Assembly.GetExecutingAssembly()); //Hangi class profile'dan kalıtım aldıysa 
                                                                     //git automapper işlemini başlat anlamına geliyor

            services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
