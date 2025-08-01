using Application.Common.Interfaces;
using Dominio.Maintenance;
using Infrastrucure.Common.Interfaces;
using Infrastrucure.Repositories.Shared;

namespace Infrastrucure.Repositories
{
    public class ApartmentRepository : Repository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(IDatabaseContext database) : base(database)
        {
        }
    }
}
