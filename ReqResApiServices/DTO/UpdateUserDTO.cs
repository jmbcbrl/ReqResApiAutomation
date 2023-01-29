using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqResApiServices.DTO
{
    public class UpdateUserDTO : CreateUserDTO
    {
        public string Id { get; set; }
    }
}
