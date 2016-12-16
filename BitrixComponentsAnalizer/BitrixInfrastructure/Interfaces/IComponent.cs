namespace BitrixComponentsAnalizer.BitrixInfrastructure.Interfaces
{
    public interface IComponent
    {
        string Category { get; set; }
        string Name { get; set; }
        bool IsExistsIntoSelectedTemplates { get; set; }
        bool IsExistsIntoDefaultTemplate { get; set; }
        bool IsExistsIntoBitrix { get; set; }
        bool IsExistsAnyWhere { get; }
    }
}
