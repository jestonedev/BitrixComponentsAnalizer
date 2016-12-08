namespace BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces
{
    public interface IComponent
    {
        string Template { get; set; }
        string Category { get; set; }
        string Name { get; set; }
    }
}
