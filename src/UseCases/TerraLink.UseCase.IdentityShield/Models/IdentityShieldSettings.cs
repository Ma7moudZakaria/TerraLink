namespace TerraLink.UseCase.IdentityShield.Models
{
    /// <summary>
    /// Comprehensive settings for IdentityShield authentication and security
    /// </summary>
    public sealed class IdentityShieldSettings
    {
        /// <summary>
        /// JWT Token Configuration
        /// </summary>
        public JwtConfiguration Jwt { get; set; } = new();

        /// <summary>
        /// Password Policy Configuration
        /// </summary>
        public PasswordConfiguration Password { get; set; } = new();

        /// <summary>
        /// Account Lockout Configuration
        /// </summary>
        public LockoutConfiguration Lockout { get; set; } = new();

        /// <summary>
        /// Session Management Configuration
        /// </summary>
        public SessionConfiguration Session { get; set; } = new();

        /// <summary>
        /// User Account Configuration
        /// </summary>
        public UserConfiguration User { get; set; } = new();
    }

    /// <summary>
    /// JWT Token settings
    /// </summary>
    public sealed class JwtConfiguration
    {
        /// <summary>
        /// Secret key for JWT token signing (minimum 32 characters)
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Token issuer (e.g., "TerraLink")
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token audience (e.g., "TerraLinkAPI")
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Access token expiration in minutes (default: 60 minutes)
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; } = 60;

        /// <summary>
        /// Refresh token expiration in days (default: 7 days)
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; } = 7;

        /// <summary>
        /// OTP token expiration in minutes (default: 10 minutes)
        /// </summary>
        public int OTPExpirationMinutes { get; set; } = 10;
    }

    /// <summary>
    /// Password hashing and policy settings
    /// </summary>
    public sealed class PasswordConfiguration
    {
        /// <summary>
        /// Secret key for password hashing operations
        /// </summary>
        public string HasherKey { get; set; } = string.Empty;

        /// <summary>
        /// Require at least one digit (0-9)
        /// </summary>
        public bool RequireDigit { get; set; } = true;

        /// <summary>
        /// Require at least one lowercase letter (a-z)
        /// </summary>
        public bool RequireLowercase { get; set; } = true;

        /// <summary>
        /// Require at least one uppercase letter (A-Z)
        /// </summary>
        public bool RequireUppercase { get; set; } = true;

        /// <summary>
        /// Require at least one non-alphanumeric character (!@#$%^&*)
        /// </summary>
        public bool RequireNonAlphanumeric { get; set; } = false;

        /// <summary>
        /// Minimum password length (default: 6)
        /// </summary>
        public int RequiredLength { get; set; } = 6;

        /// <summary>
        /// Minimum number of unique characters (default: 1)
        /// </summary>
        public int RequiredUniqueChars { get; set; } = 1;
    }

    /// <summary>
    /// Account lockout settings
    /// </summary>
    public sealed class LockoutConfiguration
    {
        /// <summary>
        /// Enable account lockout on failed login attempts
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Enable lockout for new users
        /// </summary>
        public bool AllowedForNewUsers { get; set; } = true;

        /// <summary>
        /// Number of failed login attempts before lockout (default: 5)
        /// </summary>
        public int MaxFailedAccessAttempts { get; set; } = 5;

        /// <summary>
        /// Lockout duration in minutes (default: 15 minutes)
        /// </summary>
        public int DefaultLockoutTimeSpanMinutes { get; set; } = 15;
    }

    /// <summary>
    /// Session management settings
    /// </summary>
    public sealed class SessionConfiguration
    {
        /// <summary>
        /// Enable single session per user (revoke previous tokens on new login)
        /// </summary>
        public bool EnableSingleSession { get; set; } = false;

        /// <summary>
        /// Enable concurrent session tracking
        /// </summary>
        public bool TrackConcurrentSessions { get; set; } = true;

        /// <summary>
        /// Maximum number of concurrent sessions per user (0 = unlimited)
        /// </summary>
        public int MaxConcurrentSessions { get; set; } = 0;
    }

    /// <summary>
    /// User account settings
    /// </summary>
    public sealed class UserConfiguration
    {
        /// <summary>
        /// Require unique email for each user
        /// </summary>
        public bool RequireUniqueEmail { get; set; } = false;

        /// <summary>
        /// Require email confirmation before login
        /// </summary>
        public bool RequireConfirmedEmail { get; set; } = false;

        /// <summary>
        /// Require phone number confirmation
        /// </summary>
        public bool RequireConfirmedPhoneNumber { get; set; } = false;
    }
}
