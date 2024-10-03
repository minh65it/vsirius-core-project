using IdentityApi._Repositoties;
using IdentityApi.Helpers.Utilities;
using VsiriusDataModel.DataModel;
using Microsoft.EntityFrameworkCore;

namespace IdentityApi.Configurations
{
    public static class SeedDataConfig
    {
        public static async Task Run(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var _repo = services.GetRequiredService<IRepositoryAccessor>();
                    var function = services.GetRequiredService<IFunctionUtilities>();

                    await SeedData(_repo, function);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
        public static async Task SeedData(IRepositoryAccessor _repo,  IFunctionUtilities _function)
        {
            var data = await _repo.employee.FindSingle(x => x.UserName == "Admin");
            if (data == null)
            {
                //Add Admin
                Employee seedDataAdmin = new Employee();
                seedDataAdmin.Email = "admin@vsirius.com";
                seedDataAdmin.UserName =  "Admin";
                var passwordHash = _function.HashPasswordUser("VsiriusP@ssword123");
                seedDataAdmin.PasswordHash = passwordHash;

                _repo.employee.Add(seedDataAdmin);
                try
                {
                    await _repo.employee.SaveChangesAsync();
                    //Add Role
                    if (await _repo.rolesEmployee.FindSingle(x => x.Name == "Admin") == null)
                    {
                        var role = new RolesEmployee
                        {
                            Name = "Admin",
                            NormalizedName = "Admin"
                        };
                        _repo.rolesEmployee.Add(role);
                        await _repo.rolesEmployee.SaveChangesAsync();
                    }
                    //Add RoleClaims
                    var checkClims = await _repo.roleClaimsEmployee.FindAll(x => x.RoleId == "Admin").ToListAsync();
                    if (!checkClims.Any())
                    {
                        _repo.roleClaimsEmployee.Add(new RoleClaimsEmployee
                        {
                            ClaimType = "Create",
                            RoleId = "Admin",
                        });
                        _repo.roleClaimsEmployee.Add(new RoleClaimsEmployee
                        {
                            ClaimType = "Update",
                            RoleId = "Admin",
                        });
                        _repo.roleClaimsEmployee.Add(new RoleClaimsEmployee
                        {
                            ClaimType = "Delete",
                            RoleId = "Admin",
                        });
                        _repo.roleClaimsEmployee.Add(new RoleClaimsEmployee
                        {
                            ClaimType = "Read",
                            RoleId = "Admin",
                        });
                        await _repo.roleClaimsEmployee.SaveChangesAsync();
                    }

                    //gắn quyền cho admin@vsirius.com
                    var roleClaim = await _repo.rolesEmployee.FirstOrDefaultAsync(x => x.Name == "Admin");

                    UserRolesEmployee employeeRoles = new UserRolesEmployee()
                    {
                        Email = "admin@vsirius.com",
                        RoleId = roleClaim.Id,
                        Id = Guid.NewGuid().ToString(),
                    };
                    _repo.userRolesEmployee.Add(employeeRoles);
                    await _repo.userRolesEmployee.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
