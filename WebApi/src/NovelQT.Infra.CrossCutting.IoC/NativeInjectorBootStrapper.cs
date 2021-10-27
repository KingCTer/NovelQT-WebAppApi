using NovelQT.Application.Interfaces;
using NovelQT.Application.Services;
using NovelQT.Domain.CommandHandlers;
using NovelQT.Domain.Commands;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Core.Events;
using NovelQT.Domain.Core.Notifications;
using NovelQT.Domain.EventHandlers;
using NovelQT.Domain.Events;
using NovelQT.Domain.Interfaces;
using NovelQT.Infra.CrossCutting.Bus;
using NovelQT.Infra.CrossCutting.Identity.Models;
using NovelQT.Infra.Data.EventSourcing;
using NovelQT.Infra.Data.Repository;
using NovelQT.Infra.Data.Repository.EventSourcing;
using NovelQT.Infra.Data.UoW;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NovelQT.Domain.Commands.Author;
using NovelQT.Domain.Commands.Category;
using NovelQT.Domain.Commands.Book;
using NovelQT.Application.Elasticsearch;
using NovelQT.Application.Elasticsearch.Services;
using NovelQT.Application.Elasticsearch.Hosting;
using NovelQT.Infra.Data.Context;

namespace NovelQT.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddHttpContextAccessor();
            // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Elasticsearch
            services.AddSingleton<ElasticsearchClient>();
            services.AddSingleton<ElasticsearchIndexService>();
            services.AddHostedService<ElasticsearchInitializerHostedService>();
            services.AddHostedService<DocumentIndexerHostedService>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // ASP.NET Authorization Polices
            //services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IBookAppService, BookAppService>();
            services.AddScoped<IChapterAppService, ChapterAppService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<CustomerUpdatedEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<CustomerRemovedEvent>, CustomerEventHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<RegisterNewCustomerCommand, bool>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCustomerCommand, bool>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveCustomerCommand, bool>, CustomerCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewAuthorCommand, bool>, AuthorCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewCategoryCommand, bool>, CategoryCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewBookCommand, bool>, BookCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateBookCommand, bool>, BookCommandHandler>();

            services.AddScoped<IRequestHandler<RegisterNewChapterCommand, bool>, ChapterCommandHandler>();

            // Domain - 3rd parties
            //services.AddScoped<IHttpService, HttpService>();
            //services.AddScoped<IMailService, MailService>();

            // Infra - Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();


            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();

            // Infra - Identity Services
            //services.AddTransient<IEmailSender, AuthEmailMessageSender>();
            //services.AddTransient<ISmsSender, AuthSMSMessageSender>();

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();
            //services.AddSingleton<IJwtFactory, JwtFactory>();

            
        }

        public static void RegisterContextFactory(IServiceCollection services, ApplicationDbContextFactory dbContextFactory)
        {
            services.AddSingleton(dbContextFactory);
        }
    }
}
