using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.User;

public class GetUserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
    public UserStatus UserStatus { get; set; }
}