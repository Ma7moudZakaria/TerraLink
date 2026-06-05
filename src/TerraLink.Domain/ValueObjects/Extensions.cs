using ErrorOr;

namespace TerraLink.Domain.ValueObjects
{
    public readonly record struct AttachmentExtension
    {
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".pdf", ".docx", ".doc", ".mp4", ".m4a"
        };

        public string Value { get; }

        private AttachmentExtension(string value) => Value = value;

        public static ErrorOr<AttachmentExtension> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !IsAllowed(value))
            {
                return Error.Validation("not_allowed_format");
            }

            return new AttachmentExtension(value);
        }

        public static bool IsAllowed(string extension)
        {
            return AllowedExtensions.Contains(extension.ToLower());
        }
    }
}
