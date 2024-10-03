using AutoMapper;
using IdentityApi._Repositoties;
using IdentityApi._Services.Interfaces;
using IdentityApi.Data;
using IdentityApi.Dtos.Auth;
using IdentityApi.Helpers.Utilities;
using IdentityApi.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace IdentityApi._Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryAccessor _repo;
        private readonly IdentityContext _context;
        private readonly IMapper _mapper;
        private readonly IFunctionUtilities _functionUtilities;
        private ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeService(IRepositoryAccessor repo, IMapper mapper, IFunctionUtilities functionUtilities,
             ILogService logService, IHttpContextAccessor httpContextAccessor, IdentityContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _functionUtilities = functionUtilities;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<OperationResult> CreateEmployee(EmployeeDto Dto)
        {
            var checkValid = _functionUtilities.IsValidEmail(Dto.Email);
            if (!checkValid)
            {
                return new OperationResult { Message = "Your email is not in the correct format", Success = false };
            }
            if (string.IsNullOrWhiteSpace(Dto.Password))
            {
                return new OperationResult { Message = "Your password is not in the correct format", Success = false };
            }
            if (string.IsNullOrWhiteSpace(Dto.Email))
                return new OperationResult { Message = "Your email is not null", Success = false };

            if (!string.IsNullOrWhiteSpace(Dto?.PhoneNumber))
            {
                Dto.PhoneNumber = "+1" + Dto.PhoneNumber;
                var checkValidPhone = _functionUtilities.IsValidPhone(Dto.PhoneNumber);
                if (!checkValidPhone)
                {
                    return new OperationResult { Message = "Your phone number is not in the correct format", Success = false };
                }
            }
            var checkMail = await _context.Employee.FirstOrDefaultAsync(x => x.Email.Trim() == Dto.Email.Trim());
            if (checkMail != null)
            {
                if (checkMail.IsActive == true)
                {
                    return new OperationResult { Message = "Employee already exists", Success = false };
                }
                else
                {
                        return new OperationResult { Message = "Account is locked", Success = false };
                }
            }
            Dto.IsActive = true;
            Dto.Id = Guid.NewGuid();
            if (Dto.Avatar != null)
                Dto.Avatar = @"upload/Employee/" + await _functionUtilities.UploadAsync(Dto.Avatar, @"upload/Employee", Dto.Id.ToString());
            Employee Employee = _mapper.Map<Employee>(Dto);
            if (!string.IsNullOrWhiteSpace(Dto.Password))
            {
                var passwordHash = _functionUtilities.HashPasswordUser(Dto.Password);
                Employee.PasswordHash = passwordHash;
            }
            try
            {
                _context.Employee.Add(Employee);
                await _context.SaveChangesAsync();
                if (Dto.Roles != null)
                {
                    List<UserRolesEmployee> userRolesEmployees  = new List<UserRolesEmployee>();

                    var idRole =  Dto.Roles.Join(_context.RolesEmployee.ToList(), 
                        x =>x, y => y.Name, (x,y) => y.Id).ToList();
                    idRole.ForEach(x =>
                    {
                        userRolesEmployees.Add(new UserRolesEmployee
                        {
                            Email = Dto.Email,
                            Id = x,
                        });
                    });

                     _context.UserRolesEmployee.AddRange(userRolesEmployees);
                     await _context.SaveChangesAsync();
                }
                return new OperationResult { Message = "Success register Account", Success = true };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);

                return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = false };
            }
        }

        public async Task<OperationResult> CreateRole(RolesEmployeeDto roles)
        {
            if(roles.Name == "Admin")
            {
                return new OperationResult { Message = "Admin rights already exist", Success = false };
            }
            var checkRole = await _context.RolesEmployee.FirstOrDefaultAsync(x => x.Name == roles.Name);
            if(checkRole != null)
            {
                return new OperationResult { Message = "Roles already exists!!!", Success = false };
            }
            RolesEmployee rolesMap = _mapper.Map<RolesEmployee>(roles);
            _context.RolesEmployee.Add(rolesMap);
            try
            {
                await _context.SaveChangesAsync();
                var roleE = await _context.RolesEmployee.FirstOrDefaultAsync(x => x.Name == roles.Name);
                List<RoleClaimsEmployee> claimsEmployee = new List<RoleClaimsEmployee>();
                if (roles.ClaimType.Any())
                {
                    roles.ClaimType.ForEach(x =>
                    {
                        claimsEmployee.Add(new RoleClaimsEmployee
                        {
                            RoleId = roleE.Name,
                            ClaimType = x,
                        });
                    });
                    _context.RoleClaimsEmployee.AddRange(claimsEmployee);
                    await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
                return new OperationResult { Message = "Success Add Role", Success = true };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Message = "Add error role", Success = false };
            }
        }

        public async Task<OperationResult> DeleteEmployee(string id)
        {
            try
            {
                var data = await _repo.employee.FindSingle(x => x.Id.ToString() == id);
                if(data != null)
                {
                    if (data.Avatar != null)
                    {
                        _functionUtilities.DeleteFile(data.Avatar);
                    }
                    _repo.employee.Remove(data);
                    var listRole = await _context.UserRolesEmployee.Where(x => x.Email == data.Email).ToListAsync();
                    _context.UserRolesEmployee.RemoveRange(listRole);
                    await _context.SaveChangesAsync();
                    return new OperationResult { Message = "Successful delete", Success = true };
                }
                else
                {
                    return new OperationResult { Message = "Employee not found", Success = false };
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Message = "Delete failed", Success = false };
            }
        }

        public async Task<OperationResult> DeleteRole(string id)
        {
            try
            {
                var data = await _context.RolesEmployee.FirstOrDefaultAsync(x => x.Id.ToString() == id);
                if (data != null)
                {
                    if (data.Name == "Admin")
                    {
                        return new OperationResult { Message = "Admin rights cannot be changed", Success = false };
                    }
                    _context.RolesEmployee.Remove(data);
                    var listClaim = await _context.RoleClaimsEmployee.Where(x => x.RoleId == data.Name).ToListAsync();
                    _repo.roleClaimsEmployee.RemoveMultiple(listClaim);
                    await _repo.Save();
                    return new OperationResult { Message = "Successful delete", Success = true };
                }
                else
                {
                    return new OperationResult { Message = "Role not found", Success = false };
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Message = "Delete failed", Success = false };
            }
        }

        public async Task<PaginationUtility<EmployeeDto>> GetAllEmployee(PaginationParam pagination, string inputSearch)
        {
            var predicate = PredicateBuilder.New<Employee>(x => x.IsActive == true);
            if (!string.IsNullOrWhiteSpace(inputSearch))
            {
                predicate = predicate.And(x => x.Email.ToLower().Contains(inputSearch.ToLower()) ||
                    x.UserName.ToLower().Contains(inputSearch.ToLower()) ||
                    x.Address1.ToLower().Contains(inputSearch.ToLower()) ||
                    x.Address2.ToLower().Contains(inputSearch.ToLower())
                );
            }

            try
            {
                var data = await _repo.employee.FindAll(predicate).ToListAsync();
                var result = _mapper.Map<List<EmployeeDto>>(data);
                return PaginationUtility<EmployeeDto>.Create(result, pagination.PageNumber, pagination.PageSize, true);
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }
           
        }

        public async Task<List<RolesEmployeeDto>> GetAllRole()
        {
            try
            {
                var data = await _repo.rolesEmployee.FindAll().Join(_repo.roleClaimsEmployee.FindAll(),
                x => x.Name, y => y.RoleId, (x, y) => new { x.Name, x.Id, y.ClaimType }).ToListAsync();
                var listResult = data.GroupBy(x => new { x.Name, x.Id }).Select(x => new RolesEmployeeDto
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    ClaimType = x.Select(x => x.ClaimType).ToList(),
                }).ToList();
                return listResult;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new List<RolesEmployeeDto>();
            }
            
        }

        public async Task<RolesEmployeeDto> GetDetailRole(string id)
        {
            try
            {
                var data = await _repo.rolesEmployee.FindSingle(x => x.Id.ToString() == id);
                if (data == null) return null;
                var listClaimType = await _repo.roleClaimsEmployee.FindAll(x => x.RoleId == data.Name).Select(x => x.ClaimType).ToListAsync();
                var result = _mapper.Map<RolesEmployeeDto>(data);
                result.ClaimType = listClaimType;
                return result;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new RolesEmployeeDto();
            }
           
        }

        public async Task<EmployeeDto> GetEmployeeDetail(string mail)
        {
            try
            {
                var data = await _repo.employee.FindSingle(x => x.Email == mail);
                if (data == null) return null;
                var roles = await _repo.userRolesEmployee.FindAll(x => x.Email == mail).Join(
                        _repo.rolesEmployee.FindAll(),
                        x => x.Id, y => y.Id,
                        (x, y) => new {
                            y.Name,
                        }).Select(x => x.Name).ToListAsync();
                var result = _mapper.Map<EmployeeDto>(data);
                result.Roles = roles;
                return result;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new EmployeeDto();
            }
        }

        public async Task<OperationResult> UpdateEmployee(EmployeeDto Dto)
        {
            var checkValid = _functionUtilities.IsValidEmail(Dto.Email);
            if (!checkValid)
            {
                return new OperationResult { Message = "Your email is not in the correct format", Success = false };
            }
            if (string.IsNullOrWhiteSpace(Dto.Email))
                return new OperationResult { Message = "Your email is not null", Success = false };

            var checkMail = await _repo.employee.FindSingle(x => x.Email.Trim() == Dto.Email.Trim());
            if (checkMail == null)
            {
                    return new OperationResult { Message = "Account does not exist", Success = false };
            }
            if (!string.IsNullOrWhiteSpace(Dto?.PhoneNumber))
            {
                Dto.PhoneNumber = "+1" + Dto.PhoneNumber;
                var checkValidPhone = _functionUtilities.IsValidPhone(Dto.PhoneNumber);
                if (!checkValidPhone)
                {
                    return new OperationResult { Message = "Your phone number is not in the correct format", Success = false };
                }
            }
            if (checkMail.Avatar != null)
            {
                _functionUtilities.DeleteFile(checkMail.Avatar);
            }
            _repo.employee.Remove(checkMail);
            _repo.userRolesEmployee.RemoveMultiple(await _repo.userRolesEmployee.FindAll(x => x.Email == checkMail.Email).ToListAsync());

            Dto.Id = Guid.NewGuid();
            Dto.IsActive = true;
            if (Dto.Avatar != null)
                Dto.Avatar = @"upload/Employee/" + await _functionUtilities.UploadAsync(Dto.Avatar, @"upload/Employee", Dto.Id.ToString());
            Employee Employee = _mapper.Map<Employee>(Dto);
            if(!string.IsNullOrWhiteSpace(Dto.Password))
            {
                var passwordHash = _functionUtilities.HashPasswordUser(Dto.Password);
                Employee.PasswordHash = passwordHash;
            }
            try
            {
                _repo.employee.Add(Employee);
                await _repo.employee.SaveChangesAsync();
                 if (Dto.Roles != null)
                    {
                    List<UserRolesEmployee> userRolesEmployees = new List<UserRolesEmployee>();

                    var idRole = Dto.Roles.Join(_repo.rolesEmployee.FindAll().ToList(),
                        x => x, y => y.Name, (x, y) => y.Id).ToList();
                    idRole.ForEach(x =>
                    {
                        userRolesEmployees.Add(new UserRolesEmployee
                        {
                            Email = Dto.Email,
                            Id = x,
                        });
                    });

                    _repo.userRolesEmployee.AddMultiple(userRolesEmployees);
                    await _repo.userRolesEmployee.SaveChangesAsync();
                }
                return new OperationResult { Message = "Account update successful", Success = true };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);

                return new OperationResult { Message = "An error has occurred, contact Vsirus for support", Success = false };
            }
        }

        public async Task<OperationResult> UpdateRole(RolesEmployeeDto roles)
        {
            if (roles.Name == "Admin")
            {
                return new OperationResult { Message = "Admin rights cannot be changed", Success = false };
            }
            var checkRole = await _repo.rolesEmployee.FindSingle(x => x.Name == roles.Name);
            if (checkRole == null)
            {
                return new OperationResult { Message = "Role do not exist", Success = false };
            }
            _repo.rolesEmployee.Remove(checkRole);
            var listClaim = await _repo.roleClaimsEmployee.FindAll(x => x.RoleId == checkRole.Name).ToListAsync();
            _repo.roleClaimsEmployee.RemoveMultiple(listClaim);

            RolesEmployee rolesMap = _mapper.Map<RolesEmployee>(roles);
            _repo.rolesEmployee.Add(rolesMap);
            try
            {
                await _repo.Save();
                var roleE = await _repo.rolesEmployee.FindSingle(x => x.Name == roles.Name);
                List<RoleClaimsEmployee> claimsEmployee = new List<RoleClaimsEmployee>();
                if (roles.ClaimType.Any())
                {
                    roles.ClaimType.ForEach(x =>
                    {
                        claimsEmployee.Add(new RoleClaimsEmployee
                        {
                            RoleId = roleE.Name,
                            ClaimType = x,
                        });
                    });
                    _repo.roleClaimsEmployee.AddMultiple(claimsEmployee);
                    await _repo.Save();
                }
                await _repo.Save();
                return new OperationResult { Message = "Role update successful", Success = true };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Message = "Update error role", Success = false };
            }
        }
    }
}
