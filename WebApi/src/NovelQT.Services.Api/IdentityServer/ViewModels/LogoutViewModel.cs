using NovelQT.Services.Api.IdentityServer.Models;

namespace NovelQT.Services.Api.IdentityServer.ViewModels;
public class LogoutViewModel : LogoutInputModel
{
    public bool ShowLogoutPrompt { get; set; } = true;
}
