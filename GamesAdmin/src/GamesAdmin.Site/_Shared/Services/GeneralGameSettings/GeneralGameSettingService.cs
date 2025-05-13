using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site._Shared.Services.GeneralGameSettings
{
    public interface IGeneralGameSettingService
    {
        Task<IEnumerable<GeneralGameSetting>> GetAllGames();

        Task<IEnumerable<GeneralGameSetting>> GetJackpotGames();
    }

    public class GeneralGameSettingService : IGeneralGameSettingService
    {
        private readonly IGeneralGameSettingApi generalgameSettingApi;

        public GeneralGameSettingService(IGeneralGameSettingApi generalgameSettingApi)
        {
            this.generalgameSettingApi = generalgameSettingApi;
        }

        public async Task<IEnumerable<GeneralGameSetting>> GetAllGames()
        {
            try
            {
                var games = await generalgameSettingApi.GetAll();

                return games;
            }
            catch(Exception ex)
            {
               Console.Write(ex.ToString());

                return null;
            }
        }

        public async Task<IEnumerable<GeneralGameSetting>> GetJackpotGames()
        {
            try
            {
                return await generalgameSettingApi.GetJackpotGames();
            }
            catch
            {
                return Enumerable.Empty<GeneralGameSetting>();
            }
        }
    }
}