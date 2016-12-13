using System.Collections.Generic;
using BitrixComponentsAnalizer.Common.Interfaces;

namespace UnitTests.Common
{
    internal class StubSettings : ISettings
    {
        private readonly Dictionary<string, object> _settings = new Dictionary<string, object>();

        public object GetValue(string propertyName)
        {
            return _settings.ContainsKey(propertyName) ? _settings[propertyName] : null;
        }

        public void SetValue(string propertyName, object value)
        {
            if (_settings.ContainsKey(propertyName))
            {
                _settings[propertyName] = value;
            }
            else
            {
                _settings.Add(propertyName, value);
            }
        }
    }
}
