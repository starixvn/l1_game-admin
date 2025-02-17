using System;

namespace GamesAdmin.Site.Features._Shared
{
    public class SGApiResponse<T>
    {
        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public string Seq { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public T Result { get; set; }
    }
}
