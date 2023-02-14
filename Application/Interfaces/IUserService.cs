using Application.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<bool> RegisterAsync(RegisterDTO userDto);

    public Task<string> LoginAsync(LoginDTO userDto);
}
