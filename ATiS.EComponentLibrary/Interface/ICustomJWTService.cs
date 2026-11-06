using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Dto.User;

namespace Interface
{
    public interface ICustomJWTService
    {
        Task<string> GetToken(UserReq user);
    }
}
