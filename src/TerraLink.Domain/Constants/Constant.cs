using ErrorOr;

namespace TerraLink.Domain.Constants
{
    /// <summary>
    /// Contains constant values used tEmployeeoughout the application.
    /// </summary>
    public static class Constant
    {
        public static class TokenProviders
        {
            public const string OtpTokenProvider = "ShieldOTPTokenProvider";
        }
        public static class ErrorCode
        {
            public const string NoItemEffected = "no_item_effected";
            public const string NoItemExist = "no_item_exist";
            public const string NoItemCorrect = "no_item_correct";
            public const string NoAuthorized = "not_authorized";
            public const string NotValidId = "not_valid_id";
        }

        public static class Errors
        {
            public static readonly Error CreateFaild = Error.NotFound("create_faild");
            public static readonly Error UpdateFaild = Error.NotFound("update_faild");
            public static readonly Error DeleteFaild = Error.NotFound("delete_faild");
        }
    }
}
