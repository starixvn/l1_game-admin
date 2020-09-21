using System;
using GamesAdmin.Api.GameUser.Results;
using MediatR;

namespace GamesAdmin.Api.GameUser.Requests
{
    public class UpdateSpinRequest : IRequest<UpdateMemberSpinResult>
    {
        public UpdateSpinRequest(int memberId, byte availSpins, string adminUser, DateTime expiryDate) 
        {           
            MemberId = memberId;
            AvailSpins = availSpins;
            AdminUser = adminUser;
            ExpiryDate = expiryDate;
        }

        public int MemberId { get; }

        public byte AvailSpins { get; }

        public string AdminUser { get; }

        public DateTime ExpiryDate { get; }
    }
}
