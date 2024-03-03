using PTS.Contracts.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Contracts.Versions.Dto;
public class CreateVersionRequest
{
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    public string Description { get; set; } = string.Empty;
}
