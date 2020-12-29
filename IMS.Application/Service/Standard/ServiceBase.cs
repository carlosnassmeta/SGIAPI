using AutoMapper;
using IMS.Application.Interface;
using IMS.Domain.Services.Standard;
using IMS.Infrastructure.Repository.UnitOfWork;
using System.Globalization;

namespace IMS.Application.Service.Standard
{
    public class ServiceBase : IServiceBase
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IMapper _mapper;
        protected readonly IValidationDictionary _validationDictionary;

        protected string CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture.Name;
            }
        }

        public ServiceBase(IUnitOfWork uow, IMapper mapper, IValidationDictionary validationDictionary)
        {
            _uow = uow;
            _mapper = mapper;
            _validationDictionary = validationDictionary;
        }
    }
}
