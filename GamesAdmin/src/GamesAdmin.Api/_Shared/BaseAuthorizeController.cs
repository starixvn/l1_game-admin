using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api._Shared
{
    [ApiController]
    //[ApiController, Authorize]
    public abstract class BaseAuthorizeController : ControllerBase
    {
    }
}
