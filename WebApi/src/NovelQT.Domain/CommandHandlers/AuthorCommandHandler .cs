using NovelQT.Domain.Commands;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Core.Notifications;
using NovelQT.Domain.Events;
using NovelQT.Domain.Interfaces;
using NovelQT.Domain.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using NovelQT.Domain.Commands.Author;
using NovelQT.Domain.Commands.Category;
using NovelQT.Domain.Commands.Book;

namespace NovelQT.Domain.CommandHandlers
{
    public class AuthorCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewAuthorCommand, bool>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMediatorHandler Bus;

        public AuthorCommandHandler(
            IAuthorRepository authorRepository,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications) 
            : base(uow, bus, notifications)
        {
            _authorRepository = authorRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewAuthorCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var author = new Author(Guid.NewGuid(), message.Name);

            if (_authorRepository.GetByName(author.Name) != null)
            {
                Bus.RaiseEvent(new DomainNotification(message.MessageType, "The author has already been taken."));
                return Task.FromResult(false);
            }

            _authorRepository.Add(author);

            if (Commit())
            {
                //Bus.RaiseEvent(new AuthorRegisteredEvent(author.Id, author.Name));
            }

            return Task.FromResult(true);
        }


        public void Dispose()
        {
            _authorRepository.Dispose();
        }
    }
}
