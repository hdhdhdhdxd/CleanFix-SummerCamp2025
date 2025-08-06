using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using System.Data.SqlClient;
using System.ComponentModel;
/*
public class DBPlugin
{
    [KernelFunction, Description("Listado de empresas disponibles para reformas y mantenimiento")]
    public List<Empresa> GetAllEmpresas()
    {
        var empresas = new List<Empresa>();
        using (var connection = new SqlConnection("string-de-conexion"))
        {
            connection.Open();
            var command = new SqlCommand("SELECT Id, Nombre, Direccion FROM Empresas", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                empresas.Add(new Empresa
                {

                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Direccion = reader.GetString(2)
                });
            }
        }
        return empresas;
    }

    [KernelFunction, Description("Devuelve todos los materiales de la base de datos")]
    public List<Material> GetAllMateriales()
    {
        var materiales = new List<Material>();
        using (var connection = new SqlConnection("string-de-conexion"))
        {
            connection.Open();
            var command = new SqlCommand("SELECT Id, Nombre, Tipo FROM Materiales", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                materiales.Add(new Material
                {

                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Tipo = reader.GetString(2)
                });
            }
        }
        return materiales;
    }
}
public class Empresa
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Direccion { get; set; }
}
public class Material
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Tipo { get; set; }
}


*/
