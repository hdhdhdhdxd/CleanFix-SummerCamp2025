using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace Dominio.Maintenance
{
    public class Company
    {
        string name { get; set; }
        int id { get; set; }
        double price { get; set; }
        int workTime { get; set; }
        IssueType Issue { get; set; }

        public Company()
        {
            //Constructor por defecto
        }


        //Conmtructor company
        public Company(string name, int id, double price, int workTime, IssueType issue)
        {
            this.name = name;
            this.id = id;
            this.price = price;
            this.workTime = workTime;
            Issue = issue;
        }
        //Get issue del apartment
        public List<Company> GetApartmentIssue(IssueType type)
        {
            List<Company> companies = [];
            return companies.Where(c => c.Issue == type).ToList();
        }
    }

}
