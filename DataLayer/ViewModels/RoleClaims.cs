using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class CreateRoleClaimsViewModel
    {

        public string RoleName { get; set; }
        public List<string> ?Claims { get; set; }
    }

    public class RoleClaimViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<Claim> Claims { get; set; }
    }

    public class EditRoleClaimsViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> Claims { get; set; } = new List<string>();
    }


}
