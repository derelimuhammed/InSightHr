using AutoMapper;
using Microsoft.AspNetCore.Http;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePayment;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos;
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

namespace MVC_ONION_PROJECT.APPLICATION.Services.AdvancePaymentService
{
    public class AdvancePaymentService : IAdvancePaymentService
    {
        private readonly IAdvancePaymentRepository _advancePaymentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public AdvancePaymentService(IAdvancePaymentRepository advancePaymentRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _advancePaymentRepository = advancePaymentRepository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }


        public async Task<IDataResult<AdvancePaymentDto>> AddAsync(AdvancePaymentCreateDto advancePaymentCreateDto)
        {
            var payment = _mapper.Map<AdvancePayment>(advancePaymentCreateDto);
  
            await _advancePaymentRepository.AddAsync(payment); ;
            await _advancePaymentRepository.SaveChangesAsync();
            return new SuccessDataResult<AdvancePaymentDto>(_mapper.Map<AdvancePaymentDto>(payment), "Avans başarıyla eklendi");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var department = await _advancePaymentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return new ErrorResult("Sistemde avans bulunamadı.");

            }
            else
            {
                await _advancePaymentRepository.DeleteAsync(department);
                await _advancePaymentRepository.SaveChangesAsync();
                return new SuccessResult("Avans Silme İşlemi Başarılı.");

            }
        }

        public async Task<IDataResult<List<AdvancePaymentListDto>>> GetAllAsync()
        {
            var payments = await _advancePaymentRepository.GetAllAsync();
            var OrganizationPayment = payments.Where(x => x?.Employee?.Department?.OrganizationId == Guid.Parse(_contextAccessor.HttpContext.Session.GetString("OrganizationId")));
            if (OrganizationPayment.Count() <= 0)
            {
                return new ErrorDataResult<List<AdvancePaymentListDto>>("Sistemde avans bulunamadı.");
            }
            var paymentListDto = _mapper.Map<List<AdvancePaymentListDto>>(OrganizationPayment);
            return new SuccessDataResult<List<AdvancePaymentListDto>>(paymentListDto, "Avanslar Görüntülendi");
        }

        public async Task<IDataResult<AdvancePaymentDto>> GetByIdAsync(Guid? id)
        {
            var payment = await _advancePaymentRepository.GetByIdAsync(id);
            if (payment == null)
            {
                return new ErrorDataResult<AdvancePaymentDto>("Belirtilen ID ile avans bulunamadı.");

            }
            var paymentDTO = _mapper.Map<AdvancePaymentDto>(payment);

            return new SuccessDataResult<AdvancePaymentDto>(paymentDTO, "Belirtilen ID'de avans var.");
        }

        public async Task<IDataResult<AdvancePaymentDto>> UpdateAsync(AdvancePaymentUpdateDto advancePaymentEditDTO)
        {
            var department = await _advancePaymentRepository.GetByIdAsync(advancePaymentEditDTO.Id);
            if (department == null)
            {
                return new ErrorDataResult<AdvancePaymentDto>("Avans bulunamadı.");

            }
            var departments = await _advancePaymentRepository.GetAllAsync();
            var newDepartments = departments.ToList();
            newDepartments.Remove(department);
            
            var updatedDepartment = _mapper.Map(advancePaymentEditDTO, department);
            await _advancePaymentRepository.UpdateAsync(updatedDepartment);
            await _advancePaymentRepository.SaveChangesAsync();
            return new SuccessDataResult<AdvancePaymentDto>(_mapper.Map<AdvancePaymentDto>(updatedDepartment), "Avans güncelleme başarılı");
        }
        public async Task<IDataResult<AdvancePaymentDto>> UpdateRecetedAsync(AdvancePaymentRejectedUpdateDto advancePaymentRejectedEditDTO)
        {
            var advancePayment = await _advancePaymentRepository.GetByIdAsync(advancePaymentRejectedEditDTO.Id);
            if (advancePayment == null)
            {
                return new ErrorDataResult<AdvancePaymentDto>("avans bulunamadı.");
            }
            var advancePayments = await _advancePaymentRepository.GetAllAsync();
            var newAdvancePayments = advancePayments.ToList();
            newAdvancePayments.Remove(advancePayment);
            var updatedAdvancePayments = _mapper.Map(advancePaymentRejectedEditDTO, advancePayment);
            await _advancePaymentRepository.UpdateAsync(updatedAdvancePayments);
            await _advancePaymentRepository.SaveChangesAsync();
            return new SuccessDataResult<AdvancePaymentDto>(_mapper.Map<AdvancePaymentDto>(updatedAdvancePayments), "avans güncelleme başarılı");
        }
    }
}
