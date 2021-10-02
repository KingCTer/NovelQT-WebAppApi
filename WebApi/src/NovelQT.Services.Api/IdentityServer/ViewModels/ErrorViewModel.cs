using Duende.IdentityServer.Models;

namespace NovelQT.Services.Api.IdentityServer.ViewModels;
public class ErrorViewModel
{
    public ErrorViewModel()
    {
    }

    public ErrorViewModel(string error)
    {
        Error = new ErrorMessage { Error = error };
    }

    public ErrorMessage Error { get; set; }
}
