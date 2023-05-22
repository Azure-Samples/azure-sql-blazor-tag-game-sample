using System;

#nullable enable

namespace Tag.Sample
{
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public Guid Passkey { get; set; }
        public string UserName { get; set; }
        public string TokenColor { get; set; }
        public int XLocation { get; set; }
        public int YLocation { get; set; }
    }
}

