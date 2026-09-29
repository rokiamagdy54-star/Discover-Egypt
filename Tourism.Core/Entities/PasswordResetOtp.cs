using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.Core.Entities
{
    public class PasswordResetOtp : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string CodeHash { get; set; }

        public DateTime ExpiresOn { get; set; }

        public bool IsUsed { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
