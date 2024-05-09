namespace PTS.Backend.Exceptions.Common;
public class EntityDisabledException(string message) : BadRequestException(message)
{
}
