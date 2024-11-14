using AutoMapper;
using CSRApi._Repositories.Interfaces;
using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;
using Korzh.EasyQuery.Linq;
using System.Data.Entity;
using VsiriusDataModel.DataModel;
using VSiriusSystemLog.WriteLog;

namespace CSRApi._Services.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILoggerCustom _logService;
        public ContactService(IContactRepository contactRepository, IEmployeeRepository employeeRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerCustom logService)
        {
            _contactRepository = contactRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }

        public async Task<OperationResult> Create(ContactDto dto)
        {
            await _logService.CreateLogError("test");
            var data = _mapper.Map<Contacts>(dto);
            try
            {
                _contactRepository.Add(data);
                await _contactRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Sent email contact successfully" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> Delete(string id)
        {
            var data = await _contactRepository.FindSingle(x => x.Id.ToString() == id);
            try
            {
                _contactRepository.Remove(data);
                await _contactRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Deleted Contact" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<PaginationUtility<ContactDto>> GetData(PaginationParam pagination, string keyword)
        {
            try
            {
                var data = _contactRepository.FindAll()
                      .FullTextSearchQuery(keyword)
                      .Select(x => new ContactDto()
                      {
                          Id = x.Id,
                          Content = x.Content,
                          CreateTime = x.CreateTime,
                          Email = x.Email,
                          FullName = x.FullName,
                          Phone = x.Phone,
                          Status = x.Status,
                          EmployeeId = x.Id.ToString()
                      }).OrderByDescending(x => x.CreateTime).AsNoTracking();

                return await PaginationUtility<ContactDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, true);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return null;
            }
        }

        public async Task<ContactDto> GetDetail(string id)
        {
            try
            {
                var data = _mapper.Map<ContactDto>(await _contactRepository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id)));
                if (!string.IsNullOrEmpty(data.EmployeeId))
                {
                    var result = _employeeRepository.FirstOrDefault(x => x.Id.ToString() == data.EmployeeId);
                    return new ContactDto()
                    {
                        Id = data.Id,
                        Content = data.Content,
                        CreateTime = data.CreateTime,
                        Email = data.Email,
                        FullName = data.FullName,
                        Phone = data.Phone,
                        Status = data.Status,
                        EmployeeId = result?.Id.ToString(),
                        EmployeeName = result?.FullName
                    };
                }
                return data;
            }
            catch(Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new ContactDto();
            }
        }

        public async Task<OperationResult> Update(ContactDto dto)
        {
            var data = _mapper.Map<Contacts>(dto);
            try
            {
                _contactRepository.Update(data);
                await _contactRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Update email contact successfully" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}
