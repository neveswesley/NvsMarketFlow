namespace NvsMarketFlow.Application.Requests.User;

public class ChangePasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}