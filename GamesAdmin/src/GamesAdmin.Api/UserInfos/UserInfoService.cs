using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GamesAdmin.Api.UserInfo
{
    public interface IUserInfoService
    {
        Task<IEnumerable<UserInfoModel>> FilterByUidsAndUserName(IEnumerable<string> uids, string userName, bool excludeBot);
    }

    public class UserInfoService : IUserInfoService
    {
        private readonly IGenericRepository<AccountInfoEntity> accountInfoRepository;
        private readonly IGenericRepository<AccountEntity> accountRepository;

        public UserInfoService(IGenericRepository<AccountInfoEntity> accountInfoRepository,
            IGenericRepository<AccountEntity> accountRepository)
        {
            this.accountInfoRepository = accountInfoRepository;
            this.accountRepository = accountRepository;
        }

        public async Task<IEnumerable<UserInfoModel>> FilterByUidsAndUserName(IEnumerable<string> uids, string userName, bool excludeBot)
        {
            var userNameRegex = new Regex($"(?i){userName}");
            var userInfos = await accountInfoRepository.FilterByAsync(info => uids.Contains(info.UserId) && (!excludeBot || !info.IsBot));

            var builder = Builders<AccountEntity>.Filter;
            var filter = builder.In(x => x.Id, uids.Select(uid => ObjectId.Parse(uid)));
            var matchUserName = builder.Where(user => userName == null || userNameRegex.IsMatch(user.Local.MemberName));
            filter &= matchUserName;
            var users = await accountRepository.Collection().Find(filter).ToListAsync();
            return users.Join(userInfos,
                user => user.Id.ToString(),
                userInfo => userInfo.UserId,
                (user, userInfo) => new UserInfoModel(userInfo.UserId, user.Local.Username, user.Local.MemberName, userInfo.Name, userInfo.IsBot));
        }
    }
}