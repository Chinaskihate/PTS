using PTS.Contracts.Common;

namespace PTS.Backend.Service.IService;
public interface IBaseService
{
    Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
}
