using System;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Samples.WBC.Device.Tests.Mocks
{
    public class MockConfigurationService : IConfigurationService
    {
        public object ConfigSection { get; set; }

        public object GetConfigSection(string sectionName, Type objectType)
        {
            return ConfigSection;
        }

        public bool TryGetConfigSection(string sectionName, Type objectType, out object section)
        {
            throw new NotImplementedException();
        }

        public void UpdateConfigSection(string sectionName, object section)
        {
            throw new NotImplementedException();
        }

        public bool UpdateConfigSectionString(string sectionName, string sectionString)
        {
            throw new NotImplementedException();
        }

        public bool UpdateConfigSectionString(string sectionName, string sectionString, Type objectType)
        {
            throw new NotImplementedException();
        }

        public event EventHandler ConfigurationUpdated;
        public void WriteDefault(string sectionName, object section, bool overwrite = false)
        {
            throw new NotImplementedException();
        }

        public string[] GetSections()
        {
            throw new NotImplementedException();
        }

        public string LoadConfigSection(string sectionName)
        {
            throw new NotImplementedException();
        }

        public bool ClearAll()
        {
            throw new NotImplementedException();
        }
    }
}
