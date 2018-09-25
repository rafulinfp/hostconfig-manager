using System.Collections.Generic;
using System.Runtime.InteropServices;
using HostconfigManager.Core.Models;

namespace HostconfigManager.Core.Services
{
    public interface IEnvironmentConfig
    {
        IEnumerable<Domain> LoadDomain();
        IEnumerable<TargetEnvironment> LoadEnvironment();
        bool SetEnvironment(string hostname);
        string LoadDefault();
        OSPlatform GetOSPlatform();
    }
}
