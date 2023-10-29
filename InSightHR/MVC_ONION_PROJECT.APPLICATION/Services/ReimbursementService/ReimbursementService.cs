using AutoMapper;
using Microsoft.AspNetCore.Http;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.ReimbursementDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.ReimbursementService
{
    public class ReimbursementService:IReimbursementService
    {
        private readonly IReimbursementRepository _reimbursementRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public ReimbursementService(IReimbursementRepository reimbursementRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _reimbursementRepository = reimbursementRepository;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }


        public async Task<IDataResult<ReimbursementDto>> AddAsync(ReimbursementCreateDto reimbursementCreateDto)
        {
            var reimbursement = _mapper.Map<Reimbursement>(reimbursementCreateDto);
            reimbursement.ExpenseAttachments = File.ReadAllBytes(reimbursementCreateDto.ExpenseAttachmentspath);
            await _reimbursementRepository.AddAsync(reimbursement);
            await _reimbursementRepository.SaveChangesAsync();
            return new SuccessDataResult<ReimbursementDto>(_mapper.Map<ReimbursementDto>(reimbursement), "Masraf başarıyla eklendi");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var department = await _reimbursementRepository.GetByIdAsync(id);
            if (department == null)
            {
                return new ErrorResult("Sistemde masraf bulunamadı.");

            }
            else
            {
                await _reimbursementRepository.DeleteAsync(department);
                await _reimbursementRepository.SaveChangesAsync();
                return new SuccessResult("Masraf Silme İşlemi Başarılı.");

            }
        }

        public async Task<IDataResult<List<ReimbursementListDto>>> GetAllAsync()
        {
            var reimbursements = await _reimbursementRepository.GetAllAsync();
            var OrganizationReimbursement = reimbursements.Where(x => x?.Employee?.Department?.OrganizationId == Guid.Parse(_contextAccessor.HttpContext.Session.GetString("OrganizationId")));
            if (OrganizationReimbursement.Count() <= 0)
            {
                return new ErrorDataResult<List<ReimbursementListDto>>("Sistemde masraf bulunamadı.");
            }
            var reimbursementListDto = _mapper.Map<List<ReimbursementListDto>>(OrganizationReimbursement);
            return new SuccessDataResult<List<ReimbursementListDto>>(reimbursementListDto, "Masraflar Görüntülendi");
        }

        public async Task<IDataResult<ReimbursementDto>> GetByIdAsync(Guid? id)
        {
            var reimbursement = await _reimbursementRepository.GetByIdAsync(id);
            if (reimbursement == null)
            {
                return new ErrorDataResult<ReimbursementDto>("Belirtilen ID ile masraf bulunamadı.");

            }
            var reimbursementDTO = _mapper.Map<ReimbursementDto>(reimbursement);

            return new SuccessDataResult<ReimbursementDto>(reimbursementDTO, "Belirtilen ID'de masraf var.");
        }

        public async Task<IDataResult<ReimbursementDto>> UpdateAsync(ReimbursementUpdateDto reimbursementEditDTO)
        {
            var department = await _reimbursementRepository.GetByIdAsync(reimbursementEditDTO.Id);
            if (department == null)
            {
                return new ErrorDataResult<ReimbursementDto>("Masraf bulunamadı.");

            }
            var departments = await _reimbursementRepository.GetAllAsync();
            var newDepartments = departments.ToList();
            newDepartments.Remove(department);
            if (reimbursementEditDTO.ExpenseAttachmentspath!=null)
            {
                department.ExpenseAttachments = File.ReadAllBytes(reimbursementEditDTO.ExpenseAttachmentspath);
            }
            var updatedDepartment = _mapper.Map(reimbursementEditDTO, department);
            await _reimbursementRepository.UpdateAsync(updatedDepartment);
            await _reimbursementRepository.SaveChangesAsync();
            return new SuccessDataResult<ReimbursementDto>(_mapper.Map<ReimbursementDto>(updatedDepartment), "Masraf güncelleme başarılı");
        }
        public async Task<IDataResult<ReimbursementDto>> UpdateRecetedAsync(ReimbursementRejectedUpdateDto reimbursementRejectedEditDTO)
        {
            var reimbursement = await _reimbursementRepository.GetByIdAsync(reimbursementRejectedEditDTO.Id);
            if (reimbursement == null)
            {
                return new ErrorDataResult<ReimbursementDto>("masraf bulunamadı.");
            }
            var reimbursements = await _reimbursementRepository.GetAllAsync();
            var newReimbursements = reimbursements.ToList();
            newReimbursements.Remove(reimbursement);
            var updatedReimbursements = _mapper.Map(reimbursementRejectedEditDTO, reimbursement);
            await _reimbursementRepository.UpdateAsync(updatedReimbursements);
            await _reimbursementRepository.SaveChangesAsync();
            return new SuccessDataResult<ReimbursementDto>(_mapper.Map<ReimbursementDto>(updatedReimbursements), "masraf güncelleme başarılı");
        }

        public async Task<IDataResult<List<ReimbursementListDto>>> GetListOfMineReimbursement(string identityId)
        {
            var listReimbursement = await _reimbursementRepository.GetAllAsync();
            var listMineReimbursement= listReimbursement.ToList().Where(x=>x.Employee.IdentityId==identityId);
            if (listMineReimbursement.Count() == 0)
            {
                return new ErrorDataResult<List<ReimbursementListDto>>("Harcama Bilgisi Bulunamadı.");
            }

            var reimbursementDTO = _mapper.Map<List<ReimbursementListDto>>(listMineReimbursement.ToList());
            return new SuccessDataResult<List<ReimbursementListDto>>(reimbursementDTO,"Harcamaların Listelendi.");

        }
    }
}
