using AutoMapper;
using Microsoft.AspNetCore.Http;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffTypeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;

namespace MVC_ONION_PROJECT.APPLICATION.Services.TimeOffTypeService
{
    public class TimeOffTypeService : ITimeOffTypeService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITimeOffTypeRepository _timeOffTypeRepository;

        public TimeOffTypeService(IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmployeeRepository employeeRepository, ITimeOffTypeRepository timeOffTypeRepository)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _employeeRepository = employeeRepository;
            _timeOffTypeRepository = timeOffTypeRepository;
        }

        public async Task<IDataResult<TimeOffTypeDto>> AddAsync(TimeOffTypeCreateDto timeOffTypeCreateDto)
        {
            var timeOff = _mapper.Map<TimeOffType>(timeOffTypeCreateDto);
            var identityId = _httpContextAccessor?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Kullanıcı bulunamadı";
            var employee = await _employeeRepository.GetAsync(x => x.IdentityId == identityId);

            if (employee != null)
            {
                var requestTimeOff = await _timeOffTypeRepository?.GetAllAsync();
            }

            await _timeOffTypeRepository.AddAsync(timeOff);
            await _timeOffTypeRepository.SaveChangesAsync();
            return new SuccessDataResult<TimeOffTypeDto>(_mapper.Map<TimeOffTypeDto>(timeOff), "İzin başarıyla ekldeni.");
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<List<TimeOffTypeListDto>>> GetAllAsync()
        {
            var timeOffs = await _timeOffTypeRepository.GetAllAsync();
            if (timeOffs.Count() <= 0)
            {
                return new ErrorDataResult<List<TimeOffTypeListDto>>("Sistemde izin bulunmamaktadır.");
            }
            var timeOffListDto = _mapper.Map<List<TimeOffTypeListDto>>(timeOffs);
            return new SuccessDataResult<List<TimeOffTypeListDto>>(timeOffListDto, "Listeleme başarılı");
        }

        public async Task<IDataResult<TimeOffTypeDto>> GetByIdAsync(Guid? id)
        {
            var timeOff = await _timeOffTypeRepository.GetByIdAsync(id);
            if (timeOff == null)
            {
                return new ErrorDataResult<TimeOffTypeDto>("Bu Id ile izin isteği bulunamadı.");
            }

            var timeOffDto = _mapper.Map<TimeOffTypeDto>(timeOff);
            return new SuccessDataResult<TimeOffTypeDto>(timeOffDto, "Bu Id ile izin mevcuttur");
        }

        public Task<IDataResult<TimeOffTypeDto>> UpdateAsync(TimeOffTypeUpdateDto timeOffEditDTO)
        {
            throw new NotImplementedException();
        }
    }
}
