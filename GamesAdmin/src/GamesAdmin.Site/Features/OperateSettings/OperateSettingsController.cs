using Audit.Mvc;
using GamesAdmin.Core.Models.Partners;
using GamesAdmin.Site.Features.Partners.Requests;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;
using System.Collections.Generic;
using GamesAdmin.Site.Features.OperateSettings.Requests;
using Microsoft.AspNetCore.Http;
using System;
using GamesAdmin.Site.Features.OperateSettings.ViewModels;
using GamesAdmin.Core.Models;
using System.Linq;

namespace GamesAdmin.Site.Features.OperateSettings
{
    [Authorize]
    public class OperateSettingsController : Controller
    {
        private readonly ISentryClient sentryClient;
        private readonly IOperateSettingsService operateSettingsService;
        private readonly IMediator mediator;

        public OperateSettingsController(IOperateSettingsService operateSettingsService, ISentryClient sentryClient, IMediator mediator)
        {
            this.operateSettingsService = operateSettingsService;
            this.sentryClient = sentryClient;
            this.mediator = mediator;   
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GameOperations()
        {
            return View(new GameSettingOperationViewModel
            {
                GameConfigs = await this.operateSettingsService.GetGameSettings(new List<int>())
            });
        }

        [HttpPost, Audit]
        [Route("api/OperateSettings/ExportGameConfigs")]
        public async Task<IActionResult> ExportGameConfigs([FromBody]ExportGameSettingRequest request)
        {
            if (request == null || request.GameIds == null)
            {
                return BadRequest("Invalid request data");
            }

            try 
            {
                var configurations = await operateSettingsService.GetGameSettings(request.GameIds);
                if (configurations == null)
                {
                    return NotFound("No game configurations found");
                }
                
                return Ok(configurations);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        [Route("api/OperateSettings/ImportGameConfigs")]
        public async Task<IActionResult> ImportGameConfigs([FromBody] ImportGameSettingRequest gameSettingRequest)
        {
            var importResponse = await operateSettingsService.ImportGameSetting(gameSettingRequest);

            return Json(importResponse);
        }

        [HttpGet]
        public async Task<IActionResult> CurrencyOperations()
        {
            return View(new CurrencySettingViewModel
            {
                OperatorCurrencies = await operateSettingsService.GetOperatorCurrencies(),
            });
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        [Route("api/OperateSettings/ImportCurrencies")]
        public async Task<IActionResult> ImportCurrencies([FromBody] ImportCurrenciesRequest importCurrenciesRequest)
        {
            var importResponse = await operateSettingsService.ImportCurrencies(importCurrenciesRequest);

            return Json(importResponse);
        }

        [HttpGet]
        public async Task<IActionResult> CurrencyGroupGameOperations()
        {
            return View(new CurrencyGroupGameSettingViewModel
            {
                CurrencyGroupGames = await operateSettingsService.GetCurrencyGroupGames(),
                AllGameStatusViewModel = await operateSettingsService.GetAllGames()
            });
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        [Route("api/OperateSettings/ImportCurrencyGroupGames")]
        public async Task<IActionResult> ImportCurrencyGroupGames([FromBody] ImportCurrencyGroupGameRequest importData)
        {
            try
            {
                if (importData == null || !importData.CurrencyGroupGameImportModels.Any())
                {
                    return Json(new { success = false, message = "No data to import" });
                }

                var result = await operateSettingsService.ImportCurrencyGroupGame(importData);

                return Json(new { success = true, importData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
