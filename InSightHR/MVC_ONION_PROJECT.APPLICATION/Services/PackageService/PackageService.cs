using MVC_ONION_PROJECT.APPLICATION.DTo_s.PackageDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using AutoMapper;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
namespace MVC_ONION_PROJECT.APPLICATION.Services.PackageService
{
    
    public class PackageService:IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;

        public PackageService(IPackageRepository packageRepository, IMapper mapper)
        {
            _packageRepository = packageRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<PackageDto>> GetByIdAsync(Guid? id)
        {
            var package = await _packageRepository.GetByIdAsync(id);
            if (package == null)
            {
                return new ErrorDataResult<PackageDto>("Belirtilen ID ile Paket bulunamadı.");

            }
            var packageDTO = _mapper.Map<PackageDto>(package);

            return new SuccessDataResult<PackageDto>(packageDTO, "Belirtilen ID'de Paket var.");
        }
        public async Task<IDataResult<List<PackageListDto>>> GetAllAsync()
        {
            var packages = await _packageRepository.GetAllAsync(x => x.Price,false,false);
            packages.OrderBy(x=>x.Price).ToList();
            if (packages.Count() <= 0)
            {
                return new ErrorDataResult<List<PackageListDto>>("Sistemde paket bulunamadı.");
            }
            var packageListDto = _mapper.Map<List<PackageListDto>>(packages);
            return new SuccessDataResult<List<PackageListDto>>(packageListDto, "Listeleme başarılı");
        }
        public async Task<IDataResult<PackageDto>> AddAsync(PackageCreateDto packageCreateDto)
        {

            var hasPackage = await _packageRepository.AnyAsync(x => x.PackageName == packageCreateDto.PackageName.ToLower());
            if (hasPackage)
            {
                return new ErrorDataResult<PackageDto>("Paket zaten kayıtlı");
            }

            var package = _mapper.Map<Package>(packageCreateDto);
            await _packageRepository.AddAsync(package);
            await _packageRepository.SaveChangesAsync();
            return new SuccessDataResult<PackageDto>(_mapper.Map<PackageDto>(package), "Paket Eklendi");
        }
        public async Task<IDataResult<PackageDto>> UpdateAsync(PackageUpdateDto packageeditDto)
        {
            var package = await _packageRepository.GetByIdAsync(packageeditDto.Id);
            if (package == null)
            {
                return new ErrorDataResult<PackageDto>("Paket Bulunamadi");
            }
            var packages = await _packageRepository.GetAllAsync();
            var newpackages = packages.ToList();
            newpackages.Remove(package);
            var haspackage = newpackages.Any(x => x.PackageName == packageeditDto.PackageName);
            if (haspackage)
            {
                return new ErrorDataResult<PackageDto>("Paket zaten kayitli");
            }
            var updatepackage = _mapper.Map(packageeditDto, package);
            await _packageRepository.UpdateAsync(updatepackage);
            await _packageRepository.SaveChangesAsync();
            return new SuccessDataResult<PackageDto>(_mapper.Map<PackageDto>(updatepackage), "Paket Guncelleme Basarili");
        }
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var package = await _packageRepository.GetByIdAsync(id);
            if (package is null)
            {
                return new ErrorResult("Paket Bulunamadi");
            }
            if (package.Organization.Count() > 0)
                return new ErrorResult("Paketi kullanan kullanıcı var lütfen kullanan biri varken sözleşmesi bitene kadar pasife alın");
           
            await _packageRepository.DeleteAsync(package);
            await _packageRepository.SaveChangesAsync();
            return new SuccessResult("Paket Silme islemi Basarili");
        }
    }
}
