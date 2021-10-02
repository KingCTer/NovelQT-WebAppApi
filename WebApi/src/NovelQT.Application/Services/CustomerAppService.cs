using AutoMapper;
using AutoMapper.QueryableExtensions;
using NovelQT.Application.EventSourcedNormalizers;
using NovelQT.Application.Interfaces;
using NovelQT.Application.ViewModels;
using NovelQT.Domain.Commands;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Interfaces;
using NovelQT.Infra.Data.Repository.EventSourcing;
using System;
using System.Collections.Generic;

namespace NovelQT.Application.Services
{
    public class CustomerAppService : ICustomerAppService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;

        public CustomerAppService(IMapper mapper,
                                  ICustomerRepository customerRepository,
                                  IMediatorHandler bus,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerable<CustomerViewModel> GetAll()
        {
            return _customerRepository.GetAll().ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<CustomerViewModel> GetAll(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public IList<CustomerHistoryData> GetAllHistory(Guid id)
        {
            return CustomerHistory.ToJavaScriptCustomerHistory(_eventStoreRepository.All(id));
        }

        public CustomerViewModel GetById(Guid id)
        {
            return _mapper.Map<CustomerViewModel>(_customerRepository.GetById(id));
        }

        public void Register(CustomerViewModel customerViewModel)
        {
            var registerCommand = _mapper.Map<RegisterNewCustomerCommand>(customerViewModel);
            Bus.SendCommand(registerCommand);
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(CustomerViewModel customerViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
