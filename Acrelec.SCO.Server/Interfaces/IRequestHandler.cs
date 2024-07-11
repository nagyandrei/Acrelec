using System.Net;
using System.Threading.Tasks;

namespace Acrelec.SCO.Server.Interfaces
{
    public interface IRequestHandler
    {
        Task HandleRequestAsync(HttpListenerRequest request, HttpListenerResponse response);
    }
}
