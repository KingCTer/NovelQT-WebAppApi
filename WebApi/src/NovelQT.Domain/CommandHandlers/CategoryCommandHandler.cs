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

namespace NovelQT.Domain.CommandHandlers
{
    public class CategoryCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMediatorHandler Bus;

        public CategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications) 
            : base(uow, bus, notifications)
        {
            _categoryRepository = categoryRepository;
            Bus = bus;
        }

        public Task<bool> Handle(RegisterNewCategoryCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var category = new Category(Guid.NewGuid(), message.Name);

            if (_categoryRepository.GetByName(category.Name) != null)
            {
                Bus.RaiseEvent(new DomainNotification(message.MessageType, "The category has already been taken."));
                return Task.FromResult(false);
            }

            _categoryRepository.Add(category);

            if (Commit())
            {
                //Bus.RaiseEvent(new AuthorRegisteredEvent(author.Id, author.Name));
            }

            return Task.FromResult(true);
        }


        public void Dispose()
        {
            _categoryRepository.Dispose();
        }
    }
}
