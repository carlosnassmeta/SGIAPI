using IMS.Domain.Entity;
using IMS.Domain.Repository.Domain;
using IMS.Infrastructure.Repository.Standard;
using IMS.Infrastructure.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Repository.Domain
{
    public class TruckRepository : GenericRepository<Truck>, ITruckRepository
    {
        public TruckRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Truck> GetByEmailPasswordAsync(string email, string encryptedPassword)
        {
            return await _unitOfWork.Context.Truck
                .SingleOrDefaultAsync();
        }
    }
}
