using AutoMapper;
using Microsoft.AspNetCore.Http;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.SalaryDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Concretes;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.TimeOffServices
{
    public class TimeOffService : ITimeOffService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITimeOffRepository _timeOffRepository;

        public TimeOffService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmployeeRepository employeeRepository, ITimeOffRepository timeOffRepository)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _employeeRepository = employeeRepository;
            _timeOffRepository = timeOffRepository;
        }

        public async Task<IDataResult<TimeOffDto>> AddAsync(TimeOffCreateDto timeOffCreateDto)
        {
            DateTime startDate = timeOffCreateDto.StartTime;
            DateTime endDate = timeOffCreateDto.EndTime;
            int numberOfDays = (endDate - startDate).Days;
            timeOffCreateDto.NumberOfDays = numberOfDays+1;
            var timeOff = _mapper.Map<TimeOff>(timeOffCreateDto);

            var identityId = _httpContextAccessor?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Kullanıcı bulunamadı";
            var employee = await _employeeRepository.GetAsync(x => x.IdentityId == identityId);
            if (employee == null)
                return new ErrorDataResult<TimeOffDto>( "Kişi bulanamadı ");

            //Seçilen tarih aralıklarında daha önce girilen bir izin talebi varsa uyarı verecek
            var employeeTimeOffs = await _timeOffRepository.GetAllAsync();
            if (employeeTimeOffs != null)
            {
                var thisEmployeeTimeOffs = employeeTimeOffs.Where(x => x.EmployeeId == timeOffCreateDto.EmployeeId).ToList();
                foreach (var _timeOff in thisEmployeeTimeOffs)
                {
                    if ((timeOffCreateDto.StartTime >= _timeOff.StartTime && timeOffCreateDto.StartTime <= _timeOff.EndTime) || (timeOffCreateDto.EndTime >= _timeOff.StartTime && timeOffCreateDto.EndTime <= _timeOff.EndTime))
                    {
                        return new ErrorDataResult<TimeOffDto>("Seçtiğiniz tarih aralığı içinizin talebiniz mevcut. Lütfen tarihleri kontrol ediniz.");
                    }
                }
            }

            await _timeOffRepository.AddAsync(timeOff);
            await _timeOffRepository.SaveChangesAsync();
            return new SuccessDataResult<TimeOffDto>(_mapper.Map<TimeOffDto>(timeOff), "İzin başarıyla ekldendi.");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var timeOff = await _timeOffRepository.GetByIdAsync(id);
            if (timeOff == null)
            {
                return new ErrorResult("Sistemde İzin Bulunamadı.");
            }
            else
            {
                await _timeOffRepository.DeleteAsync(timeOff);
                await _timeOffRepository.SaveChangesAsync();
                return new SuccessResult("İzin Silme İşlemi Başarılı");
            }
        }

        public async Task<IDataResult<List<TimeOffListDto>>> GetAllAsync()
        {
            var timeOffs = await _timeOffRepository.GetAllAsync();
            var OrganizationTimeOff = timeOffs.Where(x => x?.Employee?.Department?.OrganizationId == Guid.Parse(_httpContextAccessor.HttpContext.Session.GetString("OrganizationId")));

            if (OrganizationTimeOff.Count() <= 0 || OrganizationTimeOff is null)
            {
                return new ErrorDataResult<List<TimeOffListDto>>("Sistemde izin bulunmamaktadır.");
            }
            var timeOffListDto = _mapper.Map<List<TimeOffListDto>>(OrganizationTimeOff);
            return new SuccessDataResult<List<TimeOffListDto>>(timeOffListDto, "İzinler Görüntülendi");
        }

        public async Task<IDataResult<TimeOffDto>> GetByIdAsync(Guid? id)
        {
            var timeOff = await _timeOffRepository.GetByIdAsync(id);
            if (timeOff == null)
            {
                return new ErrorDataResult<TimeOffDto>("Bu Id ile izin isteği bulunamadı.");          
            }

            var timeOffDto = _mapper.Map<TimeOffDto>(timeOff);
            return new SuccessDataResult<TimeOffDto>(timeOffDto, "Bu Id ile izin mevcuttur");
        }

        public async Task<IDataResult<TimeOffDto>> UpdateAsync(TimeOffUpdateDto timeOffEditDTO)
        {
            var timeOff = await _timeOffRepository.GetByIdAsync(timeOffEditDTO.Id);
            if (timeOff == null)
            {
                return new ErrorDataResult<TimeOffDto>("izin bulunamadı.");
            }
            var timeOffs = await _timeOffRepository.GetAllAsync();
            var newTimeOffs = timeOffs.ToList();
            newTimeOffs.Remove(timeOff);
            DateTime startDate = timeOffEditDTO.StartTime;
            DateTime endDate = timeOffEditDTO.EndTime;
            int numberOfDays = (endDate - startDate).Days;
            timeOffEditDTO.NumberOfDays = numberOfDays;
            var updatedTimeOffs = _mapper.Map(timeOffEditDTO, timeOff);
            await _timeOffRepository.UpdateAsync(updatedTimeOffs);
            await _timeOffRepository.SaveChangesAsync();
            return new SuccessDataResult<TimeOffDto>(_mapper.Map<TimeOffDto>(updatedTimeOffs), "izin güncelleme başarılı");
        }
        public async Task<IDataResult<TimeOffDto>> UpdateRecetedAsync(TimeOffRejectedUpdateDto timeOffRejectedEditDTO)
        {
            var timeOff = await _timeOffRepository.GetByIdAsync(timeOffRejectedEditDTO.Id);
            if (timeOff == null)
            {
                return new ErrorDataResult<TimeOffDto>("izin bulunamadı.");
            }
            var timeOffs = await _timeOffRepository.GetAllAsync();
            var newTimeOffs = timeOffs.ToList();
            newTimeOffs.Remove(timeOff);
            var updatedTimeOffs = _mapper.Map(timeOffRejectedEditDTO, timeOff);
            await _timeOffRepository.UpdateAsync(updatedTimeOffs);
            await _timeOffRepository.SaveChangesAsync();
            return new SuccessDataResult<TimeOffDto>(_mapper.Map<TimeOffDto>(updatedTimeOffs), "izin güncelleme başarılı");
        }
    }
}
