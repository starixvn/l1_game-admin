using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api._Shared
{
    //TODO back after fixing issue on LOCAL
    //[ApiController, Authorize]
    [ApiController]
    public abstract class BaseAuthorizeController : ControllerBase
    {
    }
}
