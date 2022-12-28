using System;
using System.Collections.Generic;
using System.Linq;

namespace CompFirm.Dto.Users
{
    public class PayloadDto
    {
        public string Login { get; set; }

        public IReadOnlyCollection<string> Roles { get; set; }

        public bool IsAdmin => Roles != null
            && Roles.FirstOrDefault(x => x.Equals("Admin", StringComparison.OrdinalIgnoreCase)) != null;
    }
}
