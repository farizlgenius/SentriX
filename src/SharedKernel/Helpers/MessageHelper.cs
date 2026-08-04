using System;

namespace SharedKernel.Helpers;

public static class MessageHelper
{
      public static class Command
      {
            public static string Unsuccess(string Type, string Mac, int ComponentId) => $"{Type} [{Mac}] [{ComponentId}] - Unsuccess";
            public static string Unsuccess(string Type, string Mac) => $"{Type} [{Mac}] - Unsuccess";
      }
      public static class Common
      {

            public static string Empty(string field) => $"{field} must not be empty.";
            public static string Duplicate(string field) => $"Found duplicate {field}.";
            public static string NotFound(string field, int id) => $"{field} not found with ID {id}.";
            public static string NotFound(string field, string value) => $"{field} not found with value {value}.";
            public static string NotFound(string field, List<int> ids) => $"{field} not found with IDs {string.Join(", ", ids)}.";
            public static string PasswordLenEmpty = "Password length must be more than zero.";
            public static string Success = "Success.";
            public static string DeserializeFailed(string Method) => $"Deserialize {Method} unsuccessful";
            public static string FoundRelatedRecord(string Type) => $"Found related {Type} record.";
            public static string FoundRelatedRecord() => $"Found related record.";
            public static string SlotNotAvailable(string Table) => $"Slot for {Table} is full.";
            public static string SlotNotFound(string Guid) => $"Slot for {Guid} not found.";
      }

      public static class Auth
      {
            public const string LoginSuccess = "Login successful.";
            public const string LogoutSuccess = "Logout successful.";
            public const string InvalidCredentials = "Invalid username or password.";
            public const string UserNotFound = "User not found.";
            public const string UsernameCannotBeEmpty = "Username cannot be empty.";
            public const string PasswordCannotBeEmpty = "Password cannot be empty.";
            public const string RefreshTokenNotFound = "Refresh token not found.";
            public const string RefreshExpired = "Refresh token expired.";
            public const string RefreshTokenInvalid = "Refresh token invalid.";
            public const string GetMeSuccess = "Get Me Successful.";
            public const string RefreshTokenSuccess = "Refresh token successful.";
      }

      public static class DB
      {
            public static string SaveRecordUnsuccessful = "Save record unsuccessful.";
            public static string RecordNotFounds(string search) => $"Record not found in DB for {search}.";
            public static string RecordNotFound = "Record not found in DB.";
            public static string DeleteRecordUnsuccessful = "Delete record unsuccessful.";
            public static string UpdateRecordUnsuccessful = "Update record unsuccessful.";
            public static string QueryIdInvalid = "Query id invalid.";
            public static string DeleteRelateRecordUnsuccessful = "Delete old related record unsuccessful.";
            public static string CreateReferenceRecordUnsuccessful = "Create new related record unsuccessful.";
      }

      public static class Location
      {
            public static string LocationNotAllow = "Location not allowed.";
      }

      public static class Device
      {
            public static string DeviceMacNotFound(string mac) => $"Device with 'mac {mac}' not found.";
      }




}
