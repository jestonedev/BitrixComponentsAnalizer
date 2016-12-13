namespace BitrixComponentsAnalizer.Common.Interfaces
{
    public interface ISettings
    {
        object GetValue(string propertyName);

        void SetValue(string propertyName, object value);
    }
}
