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
        IRequestHandler<RegisterNewBookCommand, bool>,
        IRequestHandler<UpdateBookCommand, bool>
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
                message.CategoryId,
                message.IndexStatus,
                message.ChapterTotal,
                message.Intro
                );

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

        public Task<bool> Handle(UpdateBookCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var book = new Book(
                message.Id,
                message.Name,
                message.Key,
                message.Cover,
                message.Status,
                message.View,
                message.Like,
                message.AuthorId,
                message.CategoryId,
                message.IndexStatus,
                message.ChapterTotal,
                message.Intro
                );

            var existingBook = _bookRepository.GetById(book.Id);

            if (existingBook != null && existingBook.Id != book.Id)
            {
                if (!existingBook.Equals(book))
                {
                    Bus.RaiseEvent(new DomainNotification(message.MessageType, "The book has already been taken."));
                    return Task.FromResult(false);
                }
            }

            _bookRepository.Update(book);

            if (Commit())
            {
                //Bus.RaiseEvent(new CustomerUpdatedEvent(customer.Id, customer.Name, customer.Email, customer.BirthDate));
            }

            return Task.FromResult(true);
        }

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
