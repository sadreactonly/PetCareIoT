using System.Threading;
using System.Threading.Tasks;
using PetCareIoT.Models;

namespace PetCareIoT.Configurations
{
    public interface IConfigurationManager
    {
        Task<Configuration> GetAsync(CancellationToken cancellationToken);
    }
}