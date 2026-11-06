using AutoMapper;
using Interface;
using Microsoft.Extensions.Options;
using Model.Dto.Login;
using Model.Dto.User;
using Model.Entitys;
using Model.Other;
using SqlSugar;

namespace Service
{
    public class CustomJWTService : ICustomJWTService
    {
        private readonly JWTTokenOptions _JWTTokenOptions;

        public CustomJWTService(IOptionsMonitor<JWTTokenOptions> options)
        {
            _JWTTokenOptions = options.CurrentValue;
        }

        public async Task<string> GetToken(UserReq user)
        {
            return string.Empty;
        }
    }
}
