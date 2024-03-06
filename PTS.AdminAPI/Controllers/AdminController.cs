using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTS.Backend.Service.IService;
using PTS.Contracts.Auth.Dto;
using PTS.Contracts.Common;
using PTS.Contracts.Users;
using PTS.Persistence.DbContexts;
using PTS.Persistence.Models.Users;

namespace PTS.AdminAPI.Controllers;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = UserRoles.AnyAdmin)]
public class AdminController(
    IDbContextFactory<UserDbContext> dbFactory,
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    IAuthService authService) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IDbContextFactory<UserDbContext> _dbFactory = dbFactory;
    private readonly IMapper _mapper = mapper;
    private readonly IAuthService _authService = authService;
    private ResponseDto _response = new();

    [HttpGet("Users")]
    public async Task<ResponseDto> Get()
    {
        try
        {
            using var context = _dbFactory.CreateDbContext();
            var users = context.ApplicationUsers.ToList();
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                var dto = _mapper.Map<UserDto>(user);
                dto.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
                dtos.Add(dto);
            }

            _response.Result = dtos;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpGet]
    [Route("Users/{id}")]
    public async Task<ResponseDto> Get(string id)
    {
        try
        {
            using var context = _dbFactory.CreateDbContext();
            var user = context.ApplicationUsers.First(u => u.Id == id);
            var dto = _mapper.Map<UserDto>(user);
            dto.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
            _response.Result = dto;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpPost("Ban")]
    public async Task<ResponseDto> Ban(LoginRequestDto model)
    {
        try
        {
            using var context = _dbFactory.CreateDbContext();
            var user = context.ApplicationUsers.First(u => u.UserName.ToLower() == model.UserName.ToLower());
            user.IsBanned = true;
            context.ApplicationUsers.Update(user);
            context.SaveChanges();
            var response = await _authService.RevokeTokenAsync(model);
            if (response != null && response.IsSuccess && (bool)response.Result)
            {
                _response.Result = _mapper.Map<UserDto>(user);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = response.Message;
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }

    [HttpPost("Unban")]
    public ResponseDto Unban(LoginRequestDto model)
    {
        try
        {
            using var context = _dbFactory.CreateDbContext();
            var user = context.ApplicationUsers.First(u => u.UserName.ToLower() == model.UserName.ToLower());
            user.IsBanned = false;
            context.ApplicationUsers.Update(user);
            context.SaveChanges();
            _response.Result = _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return _response;
    }
}