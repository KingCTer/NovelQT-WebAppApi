using Microsoft.AspNetCore.Mvc;
using NovelQT.Domain.Common.Constants;

namespace NovelQT.Services.Api.Authorization;
public class ClaimRequirementAttribute : TypeFilterAttribute
{
    public ClaimRequirementAttribute(FunctionCode functionId, CommandCode commandId)
        : base(typeof(ClaimRequirementFilter))
    {
        Arguments = new object[] { functionId, commandId };
    }
}
