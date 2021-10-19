using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovelQT.Application.Interfaces;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Core.Notifications;
using NovelQT.Services.Api.Controllers.Base;

namespace NovelQT.Services.Api.Controllers
{
    public class BookController : MobileApiController
    {
        private readonly IBookAppService _bookAppService;
        private object customerAppService;

        public BookController(
            IBookAppService bookAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator
            ) : base(notifications, mediator)
        {
            _bookAppService = bookAppService;
        }

        [HttpPost("Crawl")]
        public IActionResult Crawl(string url)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(url);
            }

            _bookAppService.Crawl(url);

            return Response(url);
        }

        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            var bookViewModel = _bookAppService.GetById(id);

            return Response(bookViewModel);
        }

        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            return Response(_bookAppService.GetAll());
        }

        [HttpGet("pagination")]
        public IActionResult Paginationt(int skip, int take)
        {
            return Response(_bookAppService.GetAll(skip, take));
        }

    }
}
