using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Entidades;

namespace WebApi.Interfaces;
public interface IMaterial
{
    public List<Material> GetAll();
    public void Add(Material material);
    public void Update(Material material);
    public void Delete(int materialId);
}
