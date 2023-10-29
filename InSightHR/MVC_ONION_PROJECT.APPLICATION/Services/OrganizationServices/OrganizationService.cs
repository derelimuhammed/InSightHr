using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrganizationDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AddImageServices;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;

namespace MVC_ONION_PROJECT.APPLICATION.Services.OrganizationServices
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IAddImageService _addImageService;

        public OrganizationService(IMapper mapper, IOrganizationRepository organizationRepository, IAddImageService addImageService)
        {
            _mapper = mapper;
            _organizationRepository = organizationRepository;
            _addImageService = addImageService;
        }

        public async Task<IDataResult<OrganizationDto>> AddAsync(OrganizationCreateDto organizationCreateDto)
        {

            var hasOrganization = await _organizationRepository.AnyAsync(x => x.OrganizationName == organizationCreateDto.OrganizationName.ToLower());
            if (hasOrganization)
            {
                return new ErrorDataResult<OrganizationDto>("Organizasyon zaten kayıtlı");
            }

            var organization = _mapper.Map<Organization>(organizationCreateDto);
            if (organizationCreateDto.Logopath != null)
            {
                var image = await _addImageService.AddImage(organizationCreateDto.Logopath);
                if (image!=null)
                organization.logo = File.ReadAllBytes(image);
            }
            await _organizationRepository.AddAsync(organization);
            await _organizationRepository.SaveChangesAsync();
            return new SuccessDataResult<OrganizationDto>(_mapper.Map<OrganizationDto>(organization), "Organizayson Eklendi");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var organization = await _organizationRepository.GetByIdAsync(id);
            
            if (organization is null)
            {
                return new ErrorResult("Organizasyon Bulunamadi");
            }
            foreach (var item in organization.Departments)
            {
                if (item.Employees.Count>0)
                {
                    return new ErrorResult("Organizasyonun  içinde kişiler bulunmaktadır bu nedenle silinemez");
                }
            }  
            await _organizationRepository.DeleteAsync(organization);
            await _organizationRepository.SaveChangesAsync();
            return new SuccessResult("Organizasyon Silme islemi Basarili");
        }

        public async Task<IDataResult<List<OrganizationListDto>>> GetAllAsync()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            if (organizations.Count() <= 0)
            {
                return new ErrorDataResult<List<OrganizationListDto>>("Sistemde Organizasyon bulunamadı.");
            }
            var organizationListDto = _mapper.Map<List<OrganizationListDto>>(organizations);
            return new SuccessDataResult<List<OrganizationListDto>>(organizationListDto, "Listeleme başarılı");
        }

        public async Task<IDataResult<OrganizationDto>> UpdateAsync(OrganizationUpdateDto organizationeditDto)
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationeditDto.Id);
            if (organization == null)
            {
                return new ErrorDataResult<OrganizationDto>("Organizasyon Bulunamadi");
            }
            var organizations = await _organizationRepository.GetAllAsync();
            var neworganizations = organizations.ToList();
            neworganizations.Remove(organization);
            var hasorganization = neworganizations.Any(x => x.OrganizationName == organizationeditDto.OrganizationName);
            if (hasorganization)
            {
                return new ErrorDataResult<OrganizationDto>("Organizasyon zaten kayitli");
            }
            if (organizationeditDto.Logopath != null)
            {
                var image = await _addImageService.AddImage(organizationeditDto.Logopath);
                if (image != null)
                    organization.logo = File.ReadAllBytes(image);
            }
            var updateorganization = _mapper.Map(organizationeditDto, organization);
            await _organizationRepository.UpdateAsync(updateorganization);
            await _organizationRepository.SaveChangesAsync();
            return new SuccessDataResult<OrganizationDto>(_mapper.Map<OrganizationDto>(updateorganization), "Organizasyon Guncelleme Basarili");
        }

        public async Task<IDataResult<OrganizationDto>> GetByIdAsync(Guid id)
        {
            var organization = await _organizationRepository.GetByIdAsync(id);
            if (organization == null)
            {
                return new ErrorDataResult<OrganizationDto>("Belirtilen ID ile Organizasyon bulunamadı.");

            }
            var organizationDto = _mapper.Map<OrganizationDto>(organization);

            return new SuccessDataResult<OrganizationDto>(organizationDto, "Belirtilen ID'de Organizasyon var.");
        }

    }
}
