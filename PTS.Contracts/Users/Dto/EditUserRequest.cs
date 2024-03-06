using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Contracts.Users.Dto;
public class EditUserRequest
{
    public string[] Roles { get; set; }
    public bool IsBanned { get; set; }
}
