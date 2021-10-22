using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovelQT.Application.Interfaces;
using NovelQT.Application.Query;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Core.Notifications;
using NovelQT.Services.Api.Controllers.Base;

namespace NovelQT.Services.Api.Controllers
{
    public class BookController : MobileApiController
    {
        private readonly IBookAppService _bookAppService;

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
            var bookViewModels = _bookAppService.GetById(id);

            return Response(bookViewModels, 1);
        }

        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            var bookViewModels = _bookAppService.GetAll();
            return Response(bookViewModels, bookViewModels.Count());
        }

        [HttpGet("pagination")]
        public IActionResult Paginationt([FromQuery] PaginationFilter filter)
        {
            var booksResponses = _bookAppService.GetAll(filter.page_number, filter.page_size);
            return PagedResponse(booksResponses.ViewModel, booksResponses.TotalRecords, filter);
        }

    }
}
