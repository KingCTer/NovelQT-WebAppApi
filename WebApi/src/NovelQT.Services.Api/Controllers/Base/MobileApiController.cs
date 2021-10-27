using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NovelQT.Application.Query;
using NovelQT.Domain.Core.Bus;
using NovelQT.Domain.Core.Notifications;

namespace NovelQT.Services.Api.Controllers.Base
{
    [Route("api/mobile/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public abstract class MobileApiController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;

        protected MobileApiController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator
            )
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;
        }

        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

        protected bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected new IActionResult Response(object result = null, int totalRecords = 0)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    totalRecords,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                message = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected IActionResult PagedResponse(object result = null, int totalRecords = -1, PaginationFilter filter = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    pageNumber = filter.page_number + 1,
                    pageSize = filter.page_size,
                    totalPages = Convert.ToInt32(Math.Ceiling(((double)totalRecords / (double)filter.page_size))),
                    totalRecords,
                    data = result,
                    message = filter.query
                });
            }

            return BadRequest(new
            {
                success = false,
                message = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.RaiseEvent(new DomainNotification(code, message));
        }

        protected void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }
    }
}
