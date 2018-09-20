﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HostconfigManager.Core.Models;

namespace HostconfigManager.Core.Services
{
    public class EnvironmentConfig : IEnvironmentConfig
    {
        public IEnumerable<TargetEnvironment> Environments { get; set; }

        public IEnumerable<Domain> Domains { get; set; }

        public TargetEnvironment CurrentEnvironment { get; set; }

        public EnvironmentConfig()
        {
            Environments = LoadEnvironment();
            Domains = LoadDomain();
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
            List<Domain> content = new List<Domain>();
            try
            {
                using (var file = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "domain.json")))
                {
                    var contentStr = file.ReadToEnd();
                    content = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Domain>>(contentStr);
                }
            }
            catch (Exception)
            {
                ;
            }

            return content;
        }

        public bool SetEnvironment(string hostname)
        {
            // get ip address
            var ip = Environments.FirstOrDefault(s => s.Hostname == hostname)?.IpAddress;
            if (ip == null)
            {
                return false;
            }

            // generate 'hostconfig' file data
            var hostEntries = new List<string>();
            foreach (var item in Domains)
            {
                hostEntries.Add($"{ip} {item.DomainName}");
            }
            var header = $"# hosts configuration autogenerated for environment '{hostname}' {Environment.NewLine}# at '{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}' by '{Environment.UserName}'";
            try 
            {
                // create backup copy
                var hostconfigPath = @"c:\Windows\System32\Drivers\etc\hosts";
                File.Copy(hostconfigPath, hostconfigPath + ".bak", true);

                // TODO: identify OS and 

                // copy accordingly 
                
                using (var sr = new StreamWriter(hostconfigPath))
                {
                    if (hostname == "default")
                    {
                        sr.WriteLine(this.LoadDefault());
                    }
                    else
                    {
                        sr.WriteLine(header);
                        sr.WriteLine();
                        foreach (var line in hostEntries)
                        {
                            sr.WriteLine(line);
                        }
                    }

                    sr.Close();
                }

                CurrentEnvironment = Environments.FirstOrDefault(s => s.Hostname == hostname);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public string LoadDefault()
        {
            var contentStr = string.Empty;
            try
            {
                using (var file = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "hosts.default")))
                {
                    contentStr = file.ReadToEnd();
                }
            }
            catch (Exception)
            {
                ;
            }

            return contentStr;
        }
    }
}
