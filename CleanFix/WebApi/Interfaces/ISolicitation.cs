using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Entidades;

namespace WebApi.Interfaces;
public interface ISolicitation
{
    public List<Solicitation> GetAll();
    public void Add(Solicitation solicitation);
    public void Update(Solicitation solicitation);
    public void Delete(int solicitationId);
}
