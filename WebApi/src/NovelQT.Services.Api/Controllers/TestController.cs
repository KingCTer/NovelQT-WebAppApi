using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovelQT.Domain.Common.Constants;
using NovelQT.Services.Api.Authorization;
using static Duende.IdentityServer.IdentityServerConstants;
using static NovelQT.Domain.Common.Constants.MyIdentityServerConstants;

namespace NovelQT.Services.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("AllowAnonymous")]
        public IActionResult AllowAnonymous()
        {
            return Ok();
        }

        [Authorize(LocalApi.PolicyName)]
        [HttpGet]
        [Route("Authorize")]
        public IActionResult Authorize()
        {
            return Ok();
        }

        [HttpGet]
        [Route("Admin")]
        [Authorize(LocalApi.PolicyName)]
        [ClaimRequirement(FunctionCode.SYSTEM, CommandCode.CREATE)]
        public IActionResult Admin()
        {
            return Ok();
        }

        [HttpGet]
        [Route("User")]
        [Authorize(LocalApi.PolicyName)]
        [ClaimRequirement(FunctionCode.SYSTEM, CommandCode.READ)]
        public IActionResult UserTest()
        {
            return Ok();
        }
    }
}
