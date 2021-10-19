using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Core.Notifications;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using NovelQT.Domain.Commands.Book;

namespace NovelQT.Domain.CommandHandlers
{
    public class BookCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewBookCommand, bool>
        //IRequestHandler<UpdateCustomerCommand, bool>,
        //IRequestHandler<RemoveCustomerCommand, bool>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMediatorHandler Bus;

        public BookCommandHandler(
            IBookRepository bookRepository,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications) 
            : base(uow, bus, notifications)
        {
            _bookRepository = bookRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewBookCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var book = new Book(
                Guid.NewGuid(), 
                message.Name,
                message.Key,
                message.Cover,
                message.Status,
                message.View,
                message.Like,
                message.AuthorId,
                message.CategoryId);

            if (_bookRepository.GetByKey(book.Key) != null)
            {
                Bus.RaiseEvent(new DomainNotification(message.MessageType, "The book has already been taken."));
                return Task.FromResult(false);
            }

            _bookRepository.Add(book);

            if (Commit())
            {
                //Bus.RaiseEvent(new CustomerRegisteredEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));
            }

            return Task.FromResult(true);
        }

        //public Task<bool> Handle(UpdateCustomerCommand message, CancellationToken cancellationToken)
        //{
        //    if (!message.IsValid())
        //    {
        //        NotifyValidationErrors(message);
        //        return Task.FromResult(false);
        //    }

        //    var customer = new Customer(message.Id, message.Name, message.Email, message.BirthDate);
        //    var existingCustomer = _customerRepository.GetByEmail(customer.Email);

        //    if (existingCustomer != null && existingCustomer.Id != customer.Id)
        //    {
        //        if (!existingCustomer.Equals(customer))
        //        {
        //            Bus.RaiseEvent(new DomainNotification(message.MessageType, "The customer e-mail has already been taken."));
        //            return Task.FromResult(false);
        //        }
        //    }

        //    _customerRepository.Update(customer);

        //    if (Commit())
        //    {
        //        Bus.RaiseEvent(new CustomerUpdatedEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));
        //    }

        //    return Task.FromResult(true);
        //}

        //public Task<bool> Handle(RemoveCustomerCommand message, CancellationToken cancellationToken)
        //{
        //    if (!message.IsValid())
        //    {
        //        NotifyValidationErrors(message);
        //        return Task.FromResult(false);
        //    }

        //    _customerRepository.Remove(message.Id);

        //    if (Commit())
        //    {
        //        Bus.RaiseEvent(new CustomerRemovedEvent(message.Id));
        //    }

        //    return Task.FromResult(true);
        //}

        public void Dispose()
        {
            _bookRepository.Dispose();
        }
    }
}
