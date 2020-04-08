using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api._Shared
{
    [ApiController, Authorize]
    public class BaseAuthorizeController : ControllerBase
    {
    }
}
