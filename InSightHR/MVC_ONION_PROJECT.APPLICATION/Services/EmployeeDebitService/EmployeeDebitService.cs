using AutoMapper;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDebitDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using System.Transactions;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using Microsoft.AspNetCore.Http;

namespace MVC_ONION_PROJECT.APPLICATION.Services.EmployeeDebitService
{
    public class EmployeeDebitService : IEmployeeDebitService
    {
        private readonly IEmployeeDebitRepository _employeeDebitRepository;
        private readonly IOrgAssetRepository _orgAssetRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailHelper _emailHelper;

        public EmployeeDebitService(IEmployeeDebitRepository employeeDebitRepository, IMapper mapper, IOrgAssetRepository orgAssetRepository, IEmailService emailService, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor, IEmailHelper emailHelper)
        {
            _employeeDebitRepository = employeeDebitRepository;
            _mapper = mapper;
            _orgAssetRepository = orgAssetRepository;
            _emailService = emailService;
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
            _emailHelper = emailHelper;
        }



        public async Task<IDataResult<EmployeeDebitDto>> AddAsync(EmployeeDebitCreateDto employeeDebitCreateDTO)
        {

            DataResult<EmployeeDebitDto> result = new ErrorDataResult<EmployeeDebitDto>();
            var strategy = await _employeeDebitRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transactionScope = await _employeeDebitRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    employeeDebitCreateDTO.ReturnStatus = ReturnStatus.Pending;
                    var employeeDebit = _mapper.Map<EmployeeDebit>(employeeDebitCreateDTO);
                    var addDebitResult = await _employeeDebitRepository.AddAsync(employeeDebit);

                    if (addDebitResult is null)
                    {
                        result = new ErrorDataResult<EmployeeDebitDto>("Demişbaş çalışana atanamadı");
                        transactionScope.Rollback();
                        return;
                    }
                    await _employeeDebitRepository.SaveChangesAsync();
                    var employeeDebitDto = _mapper.Map<EmployeeDebitDto>(employeeDebit);
                    result = new SuccessDataResult<EmployeeDebitDto>(employeeDebitDto, "Demirbaş çalışana başarıyla atandı.");

                    //Demirbaş kişiye atandıktan sonra demirbaş atama durumu güncellenecek
                    var orgAsset = await _orgAssetRepository.GetByIdAsync(employeeDebitDto.OrgAssetId);
                    if (orgAsset == null)
                    {
                        transactionScope.Rollback();
                    }
                    orgAsset.AssetStatus = AssetStatus.PendingApproval;
                    var orgAssetRagister= await _orgAssetRepository.SaveChangesAsync();


                    var employee = await _employeeRepository.GetByIdAsync(employeeDebitDto.EmployeeId);
                    //Demirbaş kişiye atandıktan sonra bilgi maili gönderilecek
                    if (orgAssetRagister > 0)
                    {
                        if(employee.Email is null)
                        {
                            result = new ErrorDataResult<EmployeeDebitDto>("Çalışana mail adresi kayıtlı olmadığı için atanan demişbaş ile ilgili mail gönderilemedi.");
                            return;
                        }

                        var mail = await _emailHelper.ZimmetAtamaMail(orgAsset.Name);

                        await _emailService.SendMail("Zimmet Atama Hk.", mail, employee.Email);
                    }
                    else {
                        result = new ErrorDataResult<EmployeeDebitDto>("Çalışana atanan demişbaş durumu güncellenemedi. İşlem iptal edildi");
                        transactionScope.Rollback();
                        return;
                    }

                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<EmployeeDebitDto>($"Ekleme başarısız - {ex.Message}");
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;


        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
			Result result = new Result();
			var strategy = await _employeeDebitRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _employeeDebitRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
                    var employeeDebit = await _employeeDebitRepository.GetByIdAsync(id);
                    if (employeeDebit == null)
                    {
						result = new ErrorResult("Sistemde bu ID'ye ait demirbaş atama kaydı bulunamadı.");
						transactionScope.Rollback();
						return;
					}

					await _employeeDebitRepository.DeleteAsync(employeeDebit);
					await _employeeDebitRepository.SaveChangesAsync();
					result = new SuccessResult("Demirbaş çalışandan başarıyla kaldırıldı.");

					//Demirbaş kişiye atandıktan sonra demirbaş atama durumu güncellenecek
					var orgAsset = await _orgAssetRepository.GetByIdAsync(employeeDebit.OrgAssetId);
					if (orgAsset == null)
					{
						transactionScope.Rollback();
						result = new ErrorResult("Sistemde çalışan atanmış bu demirbaş için kayıt bulunamadı.");
					}
					orgAsset.AssetStatus = AssetStatus.NotAssigned;
					await _orgAssetRepository.SaveChangesAsync();

					transactionScope.Commit();
				}
				catch (Exception ex)
				{
					result = new ErrorResult($"İşlem başarısız - {ex.Message}");
					transactionScope.Rollback();
				}
				finally
				{
					transactionScope.Dispose();
				}
			});
			return result;

		}

        public async Task<IDataResult<List<EmployeeDebitListDto>>> GetAllAsync()
        {
            //var categories = await _employeeDebitRepository.GetAllAsync(x => x.CreatedDate >= DateTime.Now.AddDays(-2));
            var employeeDebits = await _employeeDebitRepository.GetAllAsync();
			var employeeDebitsOrganization = employeeDebits.Where(x => x.OrgAsset.Category.OrganizationId == Guid.Parse(_httpContextAccessor.HttpContext.Session.GetString("OrganizationId")));
            if (employeeDebitsOrganization.Count() <= 0)
                return new ErrorDataResult<List<EmployeeDebitListDto>>("Sistemde kayıtlı demirbaş atama kaydı bulunamadı.");
            var employeeDebitsListDto = _mapper.Map<List<EmployeeDebitListDto>>(employeeDebitsOrganization);
            return new SuccessDataResult<List<EmployeeDebitListDto>>(employeeDebitsListDto, "Listeleme başarılı");
        }

        public async Task<IDataResult<EmployeeDebitDto>> GetByIdAsync(Guid id)
        {
            var employeeDebit = await _employeeDebitRepository.GetByIdAsync(id);
            if (employeeDebit == null)
                return new ErrorDataResult<EmployeeDebitDto>("Kayıt bulunamadı.");
            var employeeDebitDTO = _mapper.Map<EmployeeDebitDto>(employeeDebit);
            return new SuccessDataResult<EmployeeDebitDto>(employeeDebitDTO, "Belirtilen kayıt var.");
        }

		public async Task<IDataResult<EmployeeDebitDto>> AcceptDebitAsync(Guid id)
		{
			DataResult<EmployeeDebitDto> result = new ErrorDataResult<EmployeeDebitDto>();
			var strategy = await _employeeDebitRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _employeeDebitRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var employeeDebit = await _employeeDebitRepository.GetByIdAsync(id);
					if (employeeDebit == null)
					{
						result = new ErrorDataResult<EmployeeDebitDto>("Bu zimmet atama kaydı bulunamadı.");
						transactionScope.Rollback();
						return;
					}
					employeeDebit.ReturnStatus = ReturnStatus.Assigned;
					var updateDebitResult = await _employeeDebitRepository.UpdateAsync(employeeDebit);
					if (updateDebitResult is null)
					{
						result = new ErrorDataResult<EmployeeDebitDto>("Bu zimmet atama kaydı güncellenemedi.");
						transactionScope.Rollback();
						return;
					}
					await _employeeDebitRepository.SaveChangesAsync();
					var employeeDebitDto = _mapper.Map<EmployeeDebitDto>(employeeDebit);
					result = new SuccessDataResult<EmployeeDebitDto>(employeeDebitDto, "Zimmet atama işlemini başarıyla onayladınız.");

					//Demirbaş atama durumu güncellenecek
					var orgAsset = await _orgAssetRepository.GetByIdAsync(employeeDebitDto.OrgAssetId);
					if (orgAsset == null)
					{
						transactionScope.Rollback();
					}
					orgAsset.AssetStatus = AssetStatus.Assigned;
					var orgAssetRegister = await _orgAssetRepository.SaveChangesAsync();

					if (orgAssetRegister > 0)
					{
						transactionScope.Commit();
					}
					else
					{
						transactionScope.Rollback();
						return;
					}
				}
				catch (Exception ex)
				{
					result = new ErrorDataResult<EmployeeDebitDto>($"İşlem başarısız - {ex.Message}");
					transactionScope.Rollback();
				}
				finally
				{
					transactionScope.Dispose();
				}
			});
			return result;
		}

		public async Task<IDataResult<EmployeeDebitDto>> RejectDebitAsync(Guid id)
		{
			DataResult<EmployeeDebitDto> result = new ErrorDataResult<EmployeeDebitDto>();
			var strategy = await _employeeDebitRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _employeeDebitRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var employeeDebit = await _employeeDebitRepository.GetByIdAsync(id);
					if (employeeDebit == null)
					{
						result = new ErrorDataResult<EmployeeDebitDto>("Bu zimmet atama kaydı bulunamadı.");
						transactionScope.Rollback();
						return;
					}
					employeeDebit.ReturnStatus = ReturnStatus.Rejected;
					var updateDebitResult = await _employeeDebitRepository.UpdateAsync(employeeDebit);
					if (updateDebitResult is null)
					{
						result = new ErrorDataResult<EmployeeDebitDto>("Bu zimmet atama kaydı güncellenemedi.");
						transactionScope.Rollback();
						return;
					}
					await _employeeDebitRepository.SaveChangesAsync();
					var employeeDebitDto = _mapper.Map<EmployeeDebitDto>(employeeDebit);
					result = new SuccessDataResult<EmployeeDebitDto>(employeeDebitDto, "Zimmet atama işlemini iptal ettiniz.");

					//Demirbaş atama durumu güncellenecek
					var orgAsset = await _orgAssetRepository.GetByIdAsync(employeeDebitDto.OrgAssetId);
					if (orgAsset == null)
					{
						transactionScope.Rollback();
					}
					orgAsset.AssetStatus = AssetStatus.Rejected;
					var orgAssetRegister = await _orgAssetRepository.SaveChangesAsync();

					if (orgAssetRegister > 0)
					{
						transactionScope.Commit();
					}
					else
					{
						transactionScope.Rollback();
						return;
					}			
				}
				catch (Exception ex)
				{
					result = new ErrorDataResult<EmployeeDebitDto>($"Ekleme başarısız - {ex.Message}");
					transactionScope.Rollback();
				}
				finally
				{
					transactionScope.Dispose();
				}
			});
			return result;
		}

		public async Task<IDataResult<EmployeeDebitDto>> UpdateAsync(EmployeeDebitUpdateDto employeeDebitEditDTO)
        {

            DataResult<EmployeeDebitDto> result = new ErrorDataResult<EmployeeDebitDto>();
            var strategy = await _employeeDebitRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transactionScope = await _employeeDebitRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    //çalışana atanan varlık
                    var orgAsset = await _orgAssetRepository.GetByIdAsync(employeeDebitEditDTO.OrgAssetId);
                    //çalışana atanan varlık kaydı
                    var employeeDebit = await _employeeDebitRepository.GetByIdAsync(employeeDebitEditDTO.Id);
                    if (employeeDebit == null)
                    {
                        result = new ErrorDataResult<EmployeeDebitDto>("Bu demirbaş atama kaydı bulunamadı.");
                        transactionScope.Rollback();
                        return;
                    }
                    var updatedEmployeeDebit = _mapper.Map(employeeDebitEditDTO, employeeDebit);
                    updatedEmployeeDebit.ReturnStatus = ReturnStatus.Returned;
                    var updateDebitResult = await _employeeDebitRepository.UpdateAsync(updatedEmployeeDebit);

                    if (updateDebitResult is null)
                    {
                        result = new ErrorDataResult<EmployeeDebitDto>("Demişbaş kaldırma işlemi başarısız");
                        transactionScope.Rollback();
                        return;
                    }
                    await _employeeDebitRepository.SaveChangesAsync();
                    var _employeeDebitDto = _mapper.Map<EmployeeDebitDto>(updatedEmployeeDebit);
                    result = new SuccessDataResult<EmployeeDebitDto>(_employeeDebitDto, "Demirbaş çalışandan başarıyla kaldırıldı.");



                    //Demirbaş kişiden kaldırıldıktan sonra demirbaş durumu güncellenecek                   
                    if (orgAsset == null)
                    {
                        transactionScope.Rollback();
                    }
                    orgAsset.AssetStatus = AssetStatus.NotAssigned;
                    await _orgAssetRepository.SaveChangesAsync();

                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<EmployeeDebitDto>($"Demirbaş kaldırma işlemi başarısız - {ex.Message}");
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;

        }

        public async Task<IDataResult<List<EmployeeDebitListDto>>> GetByEmployeeId(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return new ErrorDataResult<List<EmployeeDebitListDto>> ("Belirtilen ID ile Çalışan bulunamadı.");
            }
            var debitEmployee = employee.EmployeeDebit;
            if (debitEmployee == null)
            {
                return new ErrorDataResult<List<EmployeeDebitListDto>>("Belirtilen kişide zimmet bulunamadı.");
            }
            var debitEmployeeDTO = _mapper.Map<List<EmployeeDebitListDto>>(debitEmployee);

            return new SuccessDataResult<List<EmployeeDebitListDto>>(debitEmployeeDTO, "Belirtilen ID'de demirbaş mevcut.");
        }
    }
}
