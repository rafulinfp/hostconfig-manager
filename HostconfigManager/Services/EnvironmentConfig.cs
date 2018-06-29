using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HostconfigManager.Models;

namespace HostconfigManager.Services
{
    public class EnvironmentConfig : IEnvironmentConfig
    {
        public IEnumerable<TargetEnvironment> Environments { get; set; }
        public EnvironmentConfig()
        {
            Environments = LoadEnvironment();
        }

        public IEnumerable<TargetEnvironment> LoadEnvironment()
        {
            List<TargetEnvironment> content = new List<TargetEnvironment>();
            try
            {
                using (var file = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "environment.json")))
                {
                    var contentStr = file.ReadToEnd();
                    content = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TargetEnvironment>>(contentStr);
                }
            }
            catch (Exception)
            {

            }

            return content;
        }


        public IEnumerable<Domain> LoadDomain()
        {
            throw new NotImplementedException();
        }

        public bool SetEnvironment(string hostname)
        {
            return true;
        }
    }
}
