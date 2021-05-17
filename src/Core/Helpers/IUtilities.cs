using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public interface IUtilities : IAutoDependencyCore
    {
        string GenerateNumericKey(int size);

        Task<HttpResponseMessage> MakeHttpRequest(object request, string baseAddress, string requestUri, HttpMethod method, Dictionary<string, string> headers = null, bool isJson = true, bool encoded = false);
    }
}
