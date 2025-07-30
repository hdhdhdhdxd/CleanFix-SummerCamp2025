using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Entidades;

namespace WebApi.Interfaces;
public interface ICompany
{
    public List<Company> GetAll();
    public void Add(Company company);
    public void Update(Company company);
    public void Delete(int companyId);
}
