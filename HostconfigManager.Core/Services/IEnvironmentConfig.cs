﻿using System.Collections.Generic;
using HostconfigManager.Models;

namespace HostconfigManager.Core.Services
{
    public interface IEnvironmentConfig
    {
        IEnumerable<Domain> LoadDomain();
        IEnumerable<TargetEnvironment> LoadEnvironment();
        bool SetEnvironment(string hostname);
        string LoadDefault();
    }
}