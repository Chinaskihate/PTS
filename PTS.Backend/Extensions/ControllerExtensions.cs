using Microsoft.AspNetCore.Mvc;
using PTS.Contracts.Constants;

namespace PTS.Backend.Extensions;
public static class ControllerExtensions
{
    public static string GetUserId(this ControllerBase ctlr) => ctlr.Request.Headers[Constants.UserIdHeader];
}
