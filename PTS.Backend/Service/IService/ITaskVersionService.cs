using Microsoft.EntityFrameworkCore.Storage;
using PTS.Contracts.Tasks.Dto;
using PTS.Contracts.Versions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS.Backend.Service.IService;
public interface ITaskVersionService
{
    Task<TaskDto> CreateAsync(int taskId, CreateVersionRequest dto);
    Task<TaskDto> EditAsync(int taskId, int versionId, EditVersionRequest dto);
    Task<TaskDto> CreateTestCaseAsync();
    Task<TaskDto> EditTestCaseAsync();
}
