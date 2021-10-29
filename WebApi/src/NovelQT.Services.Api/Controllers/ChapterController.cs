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
    public class ChapterController : MobileApiController
    {
        private readonly IChapterAppService _chapterAppService;

        public ChapterController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator,
            IChapterAppService bookAppService
            ) : base(notifications, mediator)
        {
            _chapterAppService = bookAppService;
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetChapter(Guid id)
        {
            var response = _chapterAppService.GetChapterById(id);

            if (response == null)
            {
                return Response(response, 0);
            }

            return Response(response, 1);
        }

        [HttpGet("last/{bookId:guid}")]
        public IActionResult GetLastChapter(Guid bookId)
        {
            var response = _chapterAppService.GetLastChapterByBookId(bookId);

            if (response == null)
            {
                return Response(response, 0);
            }

            return Response(response, 1);
        }

        [HttpGet("{bookId:guid}/{order:int}")]
        public IActionResult GetChapter(Guid bookId, int order)
        {
            var response = _chapterAppService.GetChapterByBookIdAndOrder(bookId, order);

            if (response == null)
            {
                return Response(response, 0);
            }

            return Response(response, 1);
        }



        [HttpGet("list/{bookId:guid}")]
        public IActionResult GetChapterList(Guid bookId)
        {
            var chapterList = _chapterAppService.GetChapterListByBookId(bookId);
            return Response(chapterList, chapterList.Count());
        }

        [HttpGet("pagination/{bookId:guid}")]
        public IActionResult GetChapterListPagination(Guid bookId, [FromQuery] PaginationFilter filter)
        {
            var responses = _chapterAppService.GetChapterListByBookId(bookId, (filter.PageNumber - 1) * filter.PageSize, filter.PageSize, filter.Query);
            return PagedResponse(responses.ViewModel, responses.TotalRecords, filter);
        }
    }
}
