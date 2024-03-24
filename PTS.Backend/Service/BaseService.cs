using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PTS.Backend.Service.IService;
using PTS.Contracts.Common;
using Serilog;
using System.Text;

namespace PTS.Backend.Service;
public class BaseService(
    IHttpClientFactory httpClientFactory,
    ITokenProvider tokenProvider) : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
    {
        using var client = _httpClientFactory.CreateClient("PTS-API");
        HttpRequestMessage message = new();
        message.Headers.Add("Accept", "application/json");

        if (withBearer)
        {
            var token = _tokenProvider.GetToken();
            message.Headers.Add("Authorization", $"Bearer {token}");
        }

        message.RequestUri = new Uri(requestDto.Url);
        if (requestDto.Data != null)
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
        }

        HttpResponseMessage? apiResponse = null;

        switch (requestDto.ApiType)
        {
            case ApiType.GET:
                message.Method = HttpMethod.Get;
                break;
            case ApiType.POST:
                message.Method = HttpMethod.Post;
                break;
            case ApiType.PUT:
                message.Method = HttpMethod.Put;
                break;
            case ApiType.DELETE:
                message.Method = HttpMethod.Delete;
                break;
        }

        apiResponse = await client.SendAsync(message);

        try
        {
            switch (apiResponse.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message = "Not Found" };
                case System.Net.HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access denied" };
                case System.Net.HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case System.Net.HttpStatusCode.MethodNotAllowed:
                    return new() { IsSuccess = false, Message = "Method not found" };
                case System.Net.HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
            }
        }
        catch (Exception e)
        {
            Log.Error(e, e.Message);
            var dto = new ResponseDto
            {
                Message = e.Message.ToString(),
                IsSuccess = false,
            };

            return dto;
        }
    }
}
