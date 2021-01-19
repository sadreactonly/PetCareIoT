using System;
using System.IO;
using System.Threading.Tasks;

namespace PetCareIoT.Configurations
{
    public interface IConfigurationStreamProvider : IDisposable
    {
        Task<Stream> GetStreamAsync();
    }

}