using Model.Dto.User;

namespace Interface
{
    public interface ICustomJWTService
    {
        Task<string> GetToken(UserRes user);
    }
}
