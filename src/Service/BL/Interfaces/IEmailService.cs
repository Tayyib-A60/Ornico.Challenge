using Models.ViewModels;
using Services.Helpers;
using System.Threading.Tasks;

namespace Service.BL.Interfaces
{
    public interface IEmailService: IAutoDependencyService
    {
        Task<bool> SendEmail(string email, string name, string message, string subject);
    }
}
