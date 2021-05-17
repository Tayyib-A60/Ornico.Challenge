using Models.DTOs;
using Models.ViewModels;
using Services.Helpers;
using System.Threading.Tasks;

namespace Service.BL.Interfaces
{
    public interface IAuthService : IAutoDependencyService
    {
        Task<BaseResponse<UserLoginResponse>> CreateUser(CreateUserDTO createUserDTO);
        Task<BaseResponse<UserLoginResponse>> Authenticate(AuthUserDTO authUser);
        Task<BaseResponse<EmailVerificationResponse>> VerifyEmail(EmailVerificationDTO emailVerificationDTO, string userID);
        Task<BaseResponse<EmailVerificationResponse>> SendVerificationEmail(string userID);
    }
}