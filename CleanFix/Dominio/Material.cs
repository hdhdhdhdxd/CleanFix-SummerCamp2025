using Dominio.Maintenance;

namespace Dominio
{
    public class Material
    {
        //Varibales de los materiales
        string Name { get; set; }
        float Cost { get; set; }
        bool Available { get; set; }
        IssueType Issue { get; set; }
    }

}
