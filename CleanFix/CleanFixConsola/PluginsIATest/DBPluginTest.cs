using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using Microsoft.Data.SqlClient;

public class DBPluginTest
{
    [KernelFunction, Description("Obtienes las empresas de la tabla companies y puedes devolver la lista de empresas con los datos que sean necesarios en base a la peticion")]
    public List<Companies> GetAllEmpresas()
    {
        var companies = new List<Companies>();
        using (var connection = new SqlConnection("")) 
        {
            connection.Open();
            Console.WriteLine("Conexión a la base de datos establecida.");
            // Asegúrate de que la tabla Companies existe y tiene las columnas correctas
            Console.WriteLine("Ejecutando consulta para obtener empresas...");
            var command = new SqlCommand("SELECT Id, Name, Address, Number, type, Price, WorkTime FROM Companies", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                companies.Add(new Companies
                {
                    Id = reader.GetGuid(0), // Asumiendo que Id es de tipo Guid
                    Name = reader.GetString(1),
                    Address = reader.GetString(2),
                    Number = reader.GetString(3),
                    Type = reader.GetInt32(4), // Asumiendo que Type es de tipo
                    Price = reader.GetDecimal(5), // Asumiendo que Price es de tipo decimal
                    WorkTime = reader.GetInt32(6)

                });
            }
        }
        return companies;
    }

    /*[KernelFunction, Description("Devuelve todos los materiales de la base de datos")]
    public List<Materials> GetAllMateriales()
    {
        var materials = new List<Materials>();
        using (var connection = new SqlConnection("Server=(localdb)\\\\mssqllocaldb;Database=CleanFixDB;Trusted_Connection=True;MultipleActiveResultSets=true\"\n  }"))
        {
            connection.Open();
            var command = new SqlCommand("SELECT * FROM Materials", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                materials.Add(new Materials
                {

                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Cost = reader.GetFloat(2),
                    Available = reader.GetBoolean(3),
                    Issue = reader.GetInt32(4)
                });
            }
        }
        return materials;
    }*/
}
public class Companies
{

    public Guid Id { get; set; } // Identificador único de la empresa
    public string Name { get; set; }

    public string Address { get; set; }

    public string Number { get; set; }

    public int Type { get; set; }

    public decimal Price { get; set; }

    public int WorkTime { get; set; }
}
    public class Materials
{
    public int Id { get; set; }

    public string Name { get; set; }
    public float Cost { get; set; }
    public bool Available { get; set; }
    public int Issue { get; set; }
}



