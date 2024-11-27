using Cgmail.Common.Entity;

namespace Cmail.Users.Domain.Entities.Users;

public class User : AuditEntity
{
    public string Phone { get; set; } = string.Empty;
    public string PrefixPhone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public int BithDay { get; set; }
    public int BithMonth { get; set; }
    public int BithYear { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string? VerificationCode { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public DateTimeOffset? VerificationCodeExpireAt { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public bool IsReported { get; set; }
    public int? ReportCount { get; set; }
    public bool IsBlocked { get; set; }
}
