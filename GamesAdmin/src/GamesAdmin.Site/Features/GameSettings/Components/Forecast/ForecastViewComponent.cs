using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.GameSettings.Components
{
    public class ForecastViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View(new ForecastViewModel()));
        }
    }
}
