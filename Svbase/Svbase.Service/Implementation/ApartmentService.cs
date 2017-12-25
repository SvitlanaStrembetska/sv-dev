using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class ApartmentService : EntityService<IApartmentRepository, Apartment>, IApartmentService
    {
        public ApartmentService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            :base(unitOfWork,repositoryManager,repositoryManager.Apartments)
        {
            
        }

        public ApartmentViewModel GetById(int id)
        {
            var apartment = RepositoryManager.Apartments.GetById(id);
            return apartment;
        }
    }
}
