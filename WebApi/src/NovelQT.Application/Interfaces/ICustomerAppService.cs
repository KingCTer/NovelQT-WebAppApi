using NovelQT.Application.EventSourcedNormalizers;
using NovelQT.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace NovelQT.Application.Interfaces
{
    public interface ICustomerAppService : IDisposable
    {
        void Register(CustomerViewModel customerViewModel);
        IEnumerable<CustomerViewModel> GetAll();
        IEnumerable<CustomerViewModel> GetAll(int skip, int take);
        CustomerViewModel GetById(Guid id);
        void Update(CustomerViewModel customerViewModel);
        void Remove(Guid id);
        IList<CustomerHistoryData> GetAllHistory(Guid id);
    }
}
