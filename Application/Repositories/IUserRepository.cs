using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetUserByUserNameANdPaswordHashAsync(string username, string passwordHash);

    Task<bool> CheckUserNameExistedAsync(string username);
    Task<bool> CheckEmailExistedAsync(string email);

    Task<User> GetUserByEmailAsync(string email);
    Task EditRoleAsync(Guid userId, int roleId);
}
