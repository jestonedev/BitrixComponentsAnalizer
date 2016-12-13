using BitrixComponentsAnalizer.Common.Interfaces;

namespace BitrixComponentsAnalizer.Common
{
    internal class ApplicationSettings: ISettings
    {
        public object GetValue(string propertyName)
        {
            return Properties.Settings.Default[propertyName];
        }


        public void SetValue(string propertyName, object value)
        {
            Properties.Settings.Default[propertyName] = value;
            Properties.Settings.Default.Save();
        }
    }
}
